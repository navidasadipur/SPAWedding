using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPAWedding.Core.Models;
using SPAWedding.Core.Utility;

namespace SPAWedding.Infrastructure.Repositories
{
    public class ProductsRepository : BaseRepository<Product, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public ProductsRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Product> GetProducts()
        {
            return _context.Products.Where(p => p.IsDeleted == false).Include(a => a.ProductGroup).OrderByDescending(a=>a.InsertDate).ToList();
        }

        public Product GetProduct(int id)
        {
            var product = _context.Products.Include(p => p.ProductMainFeatures).Include(p => p.ProductFeatureValues).Include(p=>p.Brand)
                .Include(p=>p.SimilarProducts)
                .FirstOrDefault(p => p.Id == id);
            product.ProductMainFeatures = product.ProductMainFeatures.Where(f => f.IsDeleted == false).ToList();
            product.ProductFeatureValues = product.ProductFeatureValues.Where(f => f.IsDeleted == false).ToList();
            return product;
        }

        public List<ProductMainFeature> GetProductMainFeatures(int id)
        {
            return _context.ProductMainFeatures.Where(p=>p.ProductId == id && p.IsDeleted == false).ToList();
        }
        public List<ProductFeatureValue> GetProductFeatures(int id)
        {
            return _context.ProductFeatureValues.Where(p => p.ProductId == id && p.IsDeleted == false).ToList();
        }
        public List<SubFeature> GetSubFeaturesByFeatureId(int id)
        {
            return _context.SubFeatures.Where(p => p.IsDeleted == false && p.FeatureId == id).ToList();
        }

        public ProductMainFeature AddProductMainFeature(ProductMainFeature mainFeature)
        {
            var user = GetCurrentUser();
            mainFeature.InsertDate = DateTime.Now;
            mainFeature.InsertUser = user.UserName;
            _context.ProductMainFeatures.Add(mainFeature);
            _context.SaveChanges();

            _logger.LogEvent(mainFeature.GetType().Name, mainFeature.Id, "Add");
            return mainFeature;
        }
        public ProductFeatureValue AddProductFeature(ProductFeatureValue feature)
        {
            var user = GetCurrentUser();
            feature.InsertDate = DateTime.Now;
            feature.InsertUser = user.UserName;
            _context.ProductFeatureValues.Add(feature);
            _context.SaveChanges();

            _logger.LogEvent(feature.GetType().Name, feature.Id, "Add");
            return feature;
        }
        public List<Product> GetNewestProducts(int take)
        {
            return _context.Products.Where(p=>p.IsDeleted == false).OrderByDescending(p => p.InsertDate).Take(take).ToList();
        }


        public List<ProductColor> GetProductColors(int productId)
        {
            return _context.ProductColors.Where(c => c.ProductId == productId).ToList();
        }

        public void DeleteColorCodes(int productId)
        {
            var list = _context.ProductColors.Where(c => c.ProductId == productId).ToList();
            foreach(var item in list)
                _context.ProductColors.Remove(item);
            _context.SaveChanges();
        }


        public void AddProductColor(ProductColor productColor)
        {
            var user = GetCurrentUser();
            productColor.InsertUser = user.UserName;
            productColor.InsertDate = DateTime.Now;

            _context.ProductColors.Add(productColor);
            _context.SaveChanges();
        }


        public List<PerfumeNote> GetProductPerfumeNotes(int productId)
        {
            return _context.PerfumeNotes.Where(p => p.ProductId == productId).OrderBy(p=>p.PerfumeNoteType).ToList();
        }

        public void DeletePerfumeNote(int productId)
        {
            var list = _context.PerfumeNotes.Where(c => c.ProductId == productId).ToList();
            foreach (var item in list)
                _context.PerfumeNotes.Remove(item);
            _context.SaveChanges();
        }

        public void AddPerfumeNote(PerfumeNote perfumeNote)
        {
            var user = GetCurrentUser();
            perfumeNote.InsertUser = user.UserName;
            perfumeNote.InsertDate = DateTime.Now;


            _context.PerfumeNotes.Add(perfumeNote);
            _context.SaveChanges();
        }

        public List<AdditionalFeature> GetProductPerfumeVolumes(int productId)
        {
            return _context.AdditionalFeatures.Where(p => p.ProductId == productId && p.IsDeleted == false && p.AditionalFeatureType == AditionalFeatureType.Volume).OrderBy(p => p.Id).ToList();
        }

        public void DeletePerfumeVolume(int productId)
        {
            var list = _context.AdditionalFeatures.Where(c => c.ProductId == productId).ToList();
            foreach (var item in list)
                _context.AdditionalFeatures.Remove(item);
            _context.SaveChanges();
        }

        public void AddAdditionalFeature(AdditionalFeature additionalFeature)
        {
            var user = GetCurrentUser();
            additionalFeature.InsertUser = user.UserName;
            additionalFeature.InsertDate = DateTime.Now;


            _context.AdditionalFeatures.Add(additionalFeature);
            _context.SaveChanges();
        }

        public void UpdateSimilarProducts(int productId, List<int> similarities)
        {
            var user = GetCurrentUser();


            var prevSimilarities = (from s in _context.SimilarProducts
                                    where s.ProductId == productId
                                    select s.SimilarProductId).ToList();


            // adding new ones
            if (similarities == null)
                similarities = new List<int>();


            foreach(var id in similarities)
            {
                if (prevSimilarities.Contains(id))
                    continue;
                SimilarProduct similarProduct = new SimilarProduct();
                similarProduct.InsertUser = user.UserName;
                similarProduct.InsertDate = DateTime.Now;
                similarProduct.ProductId = productId;
                similarProduct.SimilarProductId = id;
                similarProduct.IsDeleted = false;
                _context.SimilarProducts.Add(similarProduct);
            }

            // removing those that are deleted
            foreach(var id in prevSimilarities)
            {
                if(!similarities.Contains(id))
                {
                    var similarProduct = _context.SimilarProducts.FirstOrDefault(s => s.SimilarProductId == id);
                    _context.SimilarProducts.Remove(similarProduct);
                }
            }

            _context.SaveChanges();

        }

        public List<Product> GetAllProductsWithGalleries()
        {
            var allProducts = _context.Products.Where(p => p.IsDeleted == false)
                                               .Include(a => a.ProductGroup).OrderByDescending(a => a.InsertDate).ToList();

            foreach (var product in allProducts)
            {
                product.ProductGalleries = _context.ProductGalleries.Where(pg => pg.IsDeleted == false && pg.ProductId == product.Id).ToList();
            }

            return allProducts;            
        }

        public List<Product> getProductsByGroupId(int groupId)
        {
            var allProducts = _context.Products.Where(p => p.IsDeleted == false).Include(p => p.ProductGroup);

            var ProductIdCategory = allProducts.Where(p => p.ProductGroup.Id == groupId).OrderByDescending(p => p.Id).ToList();

            return ProductIdCategory;
        }

        public List<Product> getProductsByBrandId(int brandId)
        {
            var allProducts = _context.Products.Where(p => p.IsDeleted == false).Include(p => p.Brand);

            var allProductOfOneBrand = allProducts.Where(p => p.BrandId == brandId).OrderByDescending(p => p.Id).ToList();

            return allProductOfOneBrand;
        }
    }
}
