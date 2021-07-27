using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SPAWedding.Core.Models;
using SPAWedding.Core.Utility;
using SPAWedding.Infrastructure;
using SPAWedding.Infrastructure.Helpers;
using SPAWedding.Infrastructure.Repositories;
using SPAWedding.Web.ViewModels;

namespace SPAWedding.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class StaticContentDetailsController : Controller
    {
        private readonly StaticContentDetailsRepository _repo;
        public StaticContentDetailsController(StaticContentDetailsRepository repo)
        {
            _repo = repo;
        }
        // GET: Admin/StaticContentDetails
        public ActionResult Index()
        {
            var content = _repo.GetStaticContentDetails();
            content = content.OrderByDescending(c => c.StaticContentTypeId == (int)StaticContentTypes.HomeTopSlider)
                .ThenByDescending(c => c.InsertDate).ToList();
            return View(content);
        }
        // GET: Admin/StaticContentDetails/Create
        public ActionResult Create()
        {
            ViewBag.StaticContentTypeId = new SelectList(_repo.GetStaticContentTypes(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StaticContentDetail staticContentDetail, HttpPostedFileBase StaticContentDetailImage)
        {
            if (ModelState.IsValid)
            {
                #region Upload Image
                if (StaticContentDetailImage != null)
                {
                    // Saving Temp Image
                    var newFileName = Guid.NewGuid() + Path.GetExtension(StaticContentDetailImage.FileName);
                    StaticContentDetailImage.SaveAs(Server.MapPath("/Files/StaticContentImages/Temp/" + newFileName));

                    // Resizing Image
                    ImageResizer image = new ImageResizer();
                    if (staticContentDetail.StaticContentTypeId == (int)StaticContentTypes.HomeTopSlider)
                        image = new ImageResizer(1413, 600, true);
                    if(staticContentDetail.StaticContentTypeId == (int)StaticContentTypes.PageBanner)
                        image = new ImageResizer(1450, 250, true);
                    if (staticContentDetail.StaticContentTypeId == (int)StaticContentTypes.About)
                        image = new ImageResizer(1450, 600, true);
                    if (staticContentDetail.StaticContentTypeId == (int)StaticContentTypes.HeaderFooter)
                        image = new ImageResizer(1400, 1400, true);
                    if (staticContentDetail.Id == (int)StaticContents.BackGroundImage)
                        image = new ImageResizer(2000, 1000, true);
                    if (staticContentDetail.Id == (int)StaticContents.BlogAd)
                        image = new ImageResizer(280, 280, true);
                    if (staticContentDetail.Id == (int)StaticContents.NewsBackImage)
                        image = new ImageResizer(2000, 2500, true);

                    image.Resize(Server.MapPath("/Files/StaticContentImages/Temp/" + newFileName),
                        Server.MapPath("/Files/StaticContentImages/Image/" + newFileName));

                    // Deleting Temp Image
                    System.IO.File.Delete(Server.MapPath("/Files/StaticContentImages/Temp/" + newFileName));

                    staticContentDetail.Image = newFileName;
                }
                #endregion

                _repo.Add(staticContentDetail);

                return RedirectToAction("Index");
            }
            ViewBag.StaticContentTypeId = new SelectList(_repo.GetStaticContentTypes(), "Id", "Name", staticContentDetail.StaticContentTypeId);
            return View(staticContentDetail);
        }

        // GET: Admin/StaticContentDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaticContentDetail staticContentDetail = _repo.GetStaticContentDetail(id.Value);
            if (staticContentDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.StaticContentTypeId = new SelectList(_repo.GetStaticContentTypes(), "Id", "Name", staticContentDetail.StaticContentTypeId);
            return View(staticContentDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StaticContentDetail staticContentDetail, HttpPostedFileBase StaticContentDetailImage)
        {
            if (ModelState.IsValid)
            {
                #region Upload Image
                if (StaticContentDetailImage != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("/Files/StaticContentImages/Image/" + staticContentDetail.Image)))
                        System.IO.File.Delete(Server.MapPath("/Files/StaticContentImages/Image/" + staticContentDetail.Image));

                    // Saving Temp Image
                    var newFileName = Guid.NewGuid() + Path.GetExtension(StaticContentDetailImage.FileName);
                    StaticContentDetailImage.SaveAs(Server.MapPath("/Files/StaticContentImages/Temp/" + newFileName));

                    // Resizing Image
                    ImageResizer image = new ImageResizer();
                    if (staticContentDetail.StaticContentTypeId == (int)StaticContentTypes.HomeTopSlider)
                        image = new ImageResizer(1413, 600, true);
                    if (staticContentDetail.StaticContentTypeId == (int)StaticContentTypes.PageBanner)
                        image = new ImageResizer(1450, 250, true);
                    if (staticContentDetail.StaticContentTypeId == (int)StaticContentTypes.About)
                        image = new ImageResizer(1450, 600, true);
                    if (staticContentDetail.StaticContentTypeId == (int)StaticContentTypes.HeaderFooter)
                    image = new ImageResizer(1000, 1000, true);
                    if (staticContentDetail.Id == (int)StaticContents.BackGroundImage)
                        image = new ImageResizer(2000, 1000, true);
                    if (staticContentDetail.Id == (int)StaticContents.BlogAd)
                        image = new ImageResizer(280, 280, true);
                    if (staticContentDetail.Id == (int)StaticContents.NewsBackImage)
                        image = new ImageResizer(2000, 2500, true);

                    image.Resize(Server.MapPath("/Files/StaticContentImages/Temp/" + newFileName),
                        Server.MapPath("/Files/StaticContentImages/Image/" + newFileName));

                    // Deleting Temp Image
                    System.IO.File.Delete(Server.MapPath("/Files/StaticContentImages/Temp/" + newFileName));
                    staticContentDetail.Image = newFileName;
                }
                #endregion

                _repo.Update(staticContentDetail);
                return RedirectToAction("Index");
            }
            ViewBag.StaticContentTypeId = new SelectList(_repo.GetStaticContentTypes(), "Id", "Name", staticContentDetail.StaticContentTypeId);
            return View(staticContentDetail);
        }
        // GET: Admin/StaticContentDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaticContentDetail staticContentDetail = _repo.GetStaticContentDetail(id.Value);
            if (staticContentDetail == null)
            {
                return HttpNotFound();
            }
            return PartialView(staticContentDetail);
        }

        // POST: Admin/StaticContentDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var staticContentDetail = _repo.Get(id);

            //#region Delete StaticContentDetail Image
            //if (staticContentDetail.Image != null)
            //{
            //    if (System.IO.File.Exists(Server.MapPath("/Files/StaticContentImages/Image/" + staticContentDetail.Image)))
            //        System.IO.File.Delete(Server.MapPath("/Files/StaticContentImages/Image/" + staticContentDetail.Image));

            //    if (System.IO.File.Exists(Server.MapPath("/Files/StaticContentImages/Thumb/" + staticContentDetail.Image)))
            //        System.IO.File.Delete(Server.MapPath("/Files/StaticContentImages/Thumb/" + staticContentDetail.Image));
            //}
            //#endregion

            _repo.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult EnableDisablePopup()
        {

            var popup = _repo.GetSingleContentByTypeId((int)StaticContentTypes.Popup);

            if(popup.ShortDescription.Equals("فعال"))
                popup.ShortDescription = "غیر فعال";
            else
                popup.ShortDescription = "فعال";

            _repo.Update(popup);

            return Redirect("/Admin/StaticContentDetails/");
        }

    }
}
