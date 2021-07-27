
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc;
using SPAWedding.Core.Models;
using SPAWedding.Core.Utility;
using SPAWedding.Infrastructure.Helpers;
using SPAWedding.Infrastructure.Repositories;
using SPAWedding.Web.ViewModels;

namespace SPAWedding.Web.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductsRepository _repo;
        private readonly ProductGroupsRepository _pgRepo;
        private readonly ProductFeatureValuesRepository _featureRepo;
        private readonly ProductMainFeaturesRepository _mainFeatureRepo;

        public ProductsController(ProductsRepository repo, ProductGroupsRepository pgRepo, ProductFeatureValuesRepository featureRepo, ProductMainFeaturesRepository mainFeatureRepo)
        {
            _repo = repo;
            _pgRepo = pgRepo;
            _featureRepo = featureRepo;
            _mainFeatureRepo = mainFeatureRepo;
        }

        // GET: Admin/Products
        public ActionResult Index()
        {
            return View(_repo.GetProducts());
        }
        public ActionResult Create()
        {
            ViewBag.ProductGroups = _pgRepo.GetProductGroups();
            ViewBag.Products = _repo.GetAll();
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public int? Create(NewProductViewModel product)
        {
            if (!ModelState.IsValid) return null;
            var prod = new Product();
            prod.Title = product.Title;
            prod.ShortDescription = product.ShortDescription;
            prod.Keywords = product.Keywords;
            prod.Description = HttpUtility.UrlDecode(product.Description, System.Text.Encoding.Default);
            prod.BrandId = product.Brand;
            prod.ProductGroupId = product.ProductGroup;
            prod.Rate = product.Rate;
            prod.ShortDescription = product.ShortDescription;
            prod.ShortTitle = product.ShortTitle;
            var addProduct = _repo.Add(prod);
            #region Adding Product Features

            foreach (var feature in product.ProductFeatures)
            {
                if (feature.IsMain)
                {
                    var model = new ProductMainFeature();
                    model.ProductId = addProduct.Id;
                    model.FeatureId = feature.FeatureId;
                    model.SubFeatureId = feature.SubFeatureId;
                    model.Value = feature.Value;
                    model.Quantity = feature.Quantity??0;
                    model.Price = feature.Price ?? 0;
                    model.AdditionalInfo = feature.AdditionalInfo;
                    _repo.AddProductMainFeature(model);
                }
                else
                {
                    var model = new ProductFeatureValue();
                    model.ProductId = addProduct.Id;
                    model.FeatureId = feature.FeatureId;
                    model.SubFeatureId = feature.SubFeatureId;
                    model.Value = feature.Value;
                    _repo.AddProductFeature(model);
                }
            }
            #endregion

            #region Adding Complementary Products
            _repo.UpdateSimilarProducts(addProduct.Id , product.SimilarIds);
            #endregion

            return addProduct.Id;

        }
        public ActionResult Edit(int id)
        {
            ViewBag.ProductGroups = _pgRepo.GetProductGroups();
            ViewBag.PerfumeNotes = _repo.GetProductPerfumeNotes(id);
            ViewBag.Products = _repo.GetAll();
            var product = _repo.GetProduct(id);
            return View(product);
        }
        [HttpPost, ValidateInput(false)]
        public int? Edit(NewProductViewModel product)
        {
            if (!ModelState.IsValid) return null;
            var prod = _repo.Get(product.ProductId.Value);
            prod.Title = product.Title;
            prod.ShortDescription = product.ShortDescription;
            prod.Keywords = product.Keywords;
            prod.Description = HttpUtility.UrlDecode(product.Description, System.Text.Encoding.Default);
            prod.BrandId = product.Brand;
            prod.ProductGroupId = product.ProductGroup;
            prod.Rate = product.Rate;
            prod.ShortTitle = product.ShortTitle;

            var updateProduct = _repo.Update(prod);
            #region Removing Previous Product Features
            var productMainFeatures = _repo.GetProductMainFeatures(updateProduct.Id);
            foreach (var mainFeature in productMainFeatures)
                _mainFeatureRepo.Delete(mainFeature.Id);

            var productFeatures = _repo.GetProductFeatures(updateProduct.Id);
            foreach (var feature in productFeatures)
                _featureRepo.Delete(feature.Id);
            #endregion

            #region Adding Product Features

            foreach (var feature in product.ProductFeatures)
            {
                if (feature.IsMain)
                {
                    var model = new ProductMainFeature();
                    model.ProductId = updateProduct.Id;
                    model.FeatureId = feature.FeatureId;
                    model.SubFeatureId = feature.SubFeatureId;
                    model.Value = feature.Value;
                    model.Quantity = feature.Quantity ?? 0;
                    model.Price = feature.Price ?? 0;
                    model.AdditionalInfo = feature.AdditionalInfo;
                    _repo.AddProductMainFeature(model);
                }
                else
                {
                    var model = new ProductFeatureValue();
                    model.ProductId = updateProduct.Id;
                    model.FeatureId = feature.FeatureId;
                    model.SubFeatureId = feature.SubFeatureId;
                    model.Value = feature.Value;
                    _repo.AddProductFeature(model);
                }
            }
            #endregion



            #region Adding Complementary Products
            _repo.UpdateSimilarProducts(prod.Id, product.SimilarIds);
            #endregion

            return updateProduct.Id;

        }
        [HttpPost]
        public bool UploadImage(int id, HttpPostedFileBase File)
        {
            #region Upload Image
            if (File != null)
            {
                var product = _repo.Get(id);
                if (product.Image != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("/Files/ProductGroupImages/Image/" + product.Image)))
                        System.IO.File.Delete(Server.MapPath("/Files/ProductGroupImages/Image/" + product.Image));

                    if (System.IO.File.Exists(Server.MapPath("/Files/ProductGroupImages/Thumb/" + product.Image)))
                        System.IO.File.Delete(Server.MapPath("/Files/ProductImages/Thumb/" + product.Image));
                }
                // Saving Temp Image
                var newFileName = Guid.NewGuid() + Path.GetExtension(File.FileName);
                File.SaveAs(Server.MapPath("/Files/ProductImages/Temp/" + newFileName));
                // Resize Image
                ImageResizer image = new ImageResizer(1200, 800, true);
                image.Resize(Server.MapPath("/Files/ProductImages/Temp/" + newFileName),
                    Server.MapPath("/Files/ProductImages/Image/" + newFileName));

                ImageResizer thumb = new ImageResizer(300, 300, true);
                thumb.Resize(Server.MapPath("/Files/ProductImages/Temp/" + newFileName),
                    Server.MapPath("/Files/ProductImages/Thumb/" + newFileName));

                // Deleting Temp Image
                System.IO.File.Delete(Server.MapPath("/Files/ProductImages/Temp/" + newFileName));
                product.Image = newFileName;
                _repo.Update(product);
                return true;

            }
            #endregion

            return false;
        }
        public JsonResult GetProductGroupFeatures(int id)
        {
            var features = _pgRepo.GetProductGroupFeatures(id);
            var obj = features.Select(item => new FeaturesObjViewModel() {Id = item.Id, Title = item.Title}).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool UploadAdditionalData(int id, FormCollection formCollection)
        {
            var val = formCollection["info"];
            var productFormDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductFormData>>(val);

            var count = Request.Files.Count;


            #region  getting previous information if exists
            var productColors = _repo.GetProductColors(id);
            var perfumeNotes = _repo.GetProductPerfumeNotes(id);
            var perfumeVolumes = _repo.GetProductPerfumeVolumes(id);
            #endregion


            #region Removing Previous Perfume notes
            _repo.DeletePerfumeNote(id);
            #endregion

            #region Removing Previous Color Codes
            _repo.DeleteColorCodes(id);
            #endregion

            #region Removing Perfume Volumes
            _repo.DeletePerfumeVolume(id);
            #endregion

            #region Adding Form Data Depending on Their Type
            var imageIndex = 1;
            int colorCodeIndex = 0;
            int perfumeNoteIndex = 0;
            int perfumeVolumeIndex = 0;
            foreach(var item in productFormDataList)
            {
                if(item.ObjectType == 1) // perfume note
                {
                    PerfumeNote perfumeNote = new PerfumeNote();
                    perfumeNote.PerfumeNoteType = (PerfumeNoteType)item.Type;
                    perfumeNote.Title = item.Title;
                    perfumeNote.Link = item.Link;
                    perfumeNote.ProductId = id;

                    var file = Request.Files["image" + imageIndex];
                    if(file != null)
                    {
                        var newFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        perfumeNote.Image = newFileName;

                        file.SaveAs(Server.MapPath("/Files/PerfumeNotes/Images/" + newFileName));

                        // delete existing image 
                        try
                        {
                            if (System.IO.File.Exists(Server.MapPath("/Files/PerfumeNotes/Images/" + perfumeNotes[perfumeNoteIndex].Image)))
                                System.IO.File.Delete(Server.MapPath("/Files/PerfumeNotes/Images/" + perfumeNotes[perfumeNoteIndex].Image));
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            perfumeNote.Image = perfumeNotes[perfumeNoteIndex].Image;
                        }
                        catch
                        {
                            perfumeNote.Image = "";
                        }
                    }


                    _repo.AddPerfumeNote(perfumeNote);
                    perfumeNoteIndex++;
                }
                else if(item.ObjectType == 2) // color code
                {
                    ProductColor productColor = new ProductColor();
                    productColor.ColorCode = item.Additional;
                    productColor.Title = item.Title;
                    productColor.Link = item.Link;
                    productColor.ProductId = id;

                    var file = Request.Files["image" + imageIndex];
                    if (file != null)
                    {
                        var newFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        productColor.Image = newFileName;

                        file.SaveAs(Server.MapPath("/Files/ProductColors/Images/" + newFileName));

                        // delete existing image 
                        try
                        {
                            if (System.IO.File.Exists(Server.MapPath("/Files/PerfumeNotes/Images/" + productColors[colorCodeIndex].Image)))
                                System.IO.File.Delete(Server.MapPath("/Files/PerfumeNotes/Images/" + productColors[colorCodeIndex].Image));
                        }
                        catch
                        {

                        }

                    }
                    else
                    {
                        try
                        {
                            productColor.Image = productColors[colorCodeIndex].Image;
                        }
                        catch
                        {
                            productColor.Image = "";
                        }
                    }


                    _repo.AddProductColor(productColor);
                    colorCodeIndex++;
                }
                else //additional features
                {

                    AdditionalFeature additionalFeature = new AdditionalFeature();
                    additionalFeature.Value = item.Volume;
                    additionalFeature.Link = item.Link;
                    additionalFeature.ProductId = id;
                    additionalFeature.AditionalFeatureType = (AditionalFeatureType)item.AdditionalFeatureType;

                    var file = Request.Files["image" + imageIndex];
                    if (file != null)
                    {
                        var newFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        additionalFeature.Image = newFileName;

                        file.SaveAs(Server.MapPath("/Files/ProductAdditionalFeatures/Images/" + newFileName));

                        // delete existing image 
                        try
                        {
                            if (System.IO.File.Exists(Server.MapPath("/Files/ProductAdditionalFeatures/Images/" + perfumeVolumes[perfumeVolumeIndex].Image)))
                                System.IO.File.Delete(Server.MapPath("/Files/ProductAdditionalFeatures/Images/" + perfumeVolumes[perfumeVolumeIndex].Image));
                        }
                        catch
                        {

                        }

                    }
                    else
                    {
                        try
                        {
                            additionalFeature.Image = perfumeVolumes[perfumeVolumeIndex].Image;
                        }
                        catch
                        {
                            additionalFeature.Image = "";
                        }
                    }


                    _repo.AddAdditionalFeature(additionalFeature);
                    perfumeVolumeIndex++;

                }



                imageIndex++;
            }
            #endregion


            return true;
        }

        public JsonResult GetProductColorCodes(int id)
        {
            var colorCodes = _repo.GetProductColors(id);
            return Json(colorCodes.GroupBy(c=>c.Id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductPerfumeNotes(int id)
        {
            var perfumeNotes = _repo.GetProductPerfumeNotes(id);
            return Json(perfumeNotes.GroupBy(c => c.Id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductPerfumePerfumeNotes(int id)
        {
            var perfumeVolumes = _repo.GetProductPerfumeVolumes(id);
            return Json(perfumeVolumes.GroupBy(c => c.Id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductFeatures(int id)
        {
            var mainFeatures = _repo.GetProductMainFeatures(id);
            var features = _repo.GetProductFeatures(id);
            var obj = mainFeatures.Select(mainFeature => new ProductFeaturesViewModel()
                {
                    ProductId = mainFeature.ProductId,
                    FeatureId = mainFeature.FeatureId,
                    SubFeatureId = mainFeature.SubFeatureId,
                    IsMain = true,
                    Value = mainFeature.Value,
                    Quantity = mainFeature.Quantity,
                    Price = mainFeature.Price,
                    AdditionalInfo = mainFeature.AdditionalInfo
            })
                .ToList();
            obj.AddRange(features.Select(feature => new ProductFeaturesViewModel() 
                {
                    ProductId = feature.ProductId, 
                    FeatureId = feature.FeatureId.Value, 
                    Value = feature.Value,
                    IsMain = false,
                    SubFeatureId = feature.SubFeatureId
                }));

            return Json(obj.GroupBy(a=>a.FeatureId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductGroupBrands(int id)
        {
            var brands = _pgRepo.GetProductGroupBrands(id);
            var obj = brands.Select(item => new BrandsObjViewModel() { Id = item.Id, Name = item.Name }).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFeatureSubFeatures(int id)
        {
            var subFeatures = _repo.GetSubFeaturesByFeatureId(id);
            var obj = subFeatures.Select(item => new SubFeaturesObjViewModel() {Id = item.Id, Value = item.Value}).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = _repo.Get(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return PartialView(product);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = _repo.Get(id);
            #region Deleting Product Features
            var productMainFeatures = _repo.GetProductMainFeatures(product.Id);
            foreach (var mainFeature in productMainFeatures)
                _mainFeatureRepo.Delete(mainFeature.Id);

            var productFeatures = _repo.GetProductFeatures(product.Id);
            foreach (var feature in productFeatures)
                _featureRepo.Delete(feature.Id);
            #endregion

            #region Removing Product Perfume notes
            _repo.DeletePerfumeNote(id);
            #endregion

            #region Removing Product Color Codes
            _repo.DeleteColorCodes(id);
            #endregion

            #region Removing Product Perfume Volumes
            _repo.DeletePerfumeVolume(id);
            #endregion

            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }

    public struct ProductFormData
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Additional { get; set; }
        public int ObjectType { get; set; } // 1: perfume note, 2: color code 3: additional features
        public int Type { get; set; } // -1: not important, 1: beginning note, 2: middle note, 3: ending note
        public string Volume { get; set; } // for perfumes
        public int AdditionalFeatureType { get; set; }
    }
}