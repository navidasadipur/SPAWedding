using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPAWedding.Core.Models;

namespace SPAWedding.Infrastructure.Repositories
{
    public class FaqGroupsRepository : BaseRepository<FaqGroup, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public FaqGroupsRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public string GetFaqGroupTitle(int groupId)
        {
            return _context.FaqGroups.Find(groupId).Title;
        }

        public FaqGroup GetFaqGroup(int id)
        {
            var fg = _context.FaqGroups.Include(p => p.Faqs).FirstOrDefault(f => f.Id == id && f.IsDeleted == false);
            fg.Faqs = fg.Faqs.Where(b => b.IsDeleted == false).ToList();
            return fg;
        }

        //public List<Feature> GetFeatures()
        //{
        //    return _context.Features.Where(f => f.IsDeleted == false).ToList();
        //}

        public List<Faq> GetAllFaqbyFaqGroupId(int groupId)
        {
            var faqs = _context.Faqs.Where(f => f.IsDeleted == false && f.FaqGroupId == groupId).Include(f => f.faqGroup).ToList();

            return faqs;
        }

        public List<FaqGroup> GetAllFaqGroups()
        {
            return _context.FaqGroups.Where(fq => fq.IsDeleted == false).ToList();
        }

        public List<FaqGroup> GetAllFaqGroupsWithFaqs()
        {
            var allFaqGroups = _context.FaqGroups.Where(fq => fq.IsDeleted == false).Include(fq => fq.Faqs).ToList();

            return allFaqGroups;
        }


        //public List<Brand> GetProductGroupBrands(int id)
        //{
        //    var pgBrands = _context.ProductGroupBrands.Where(f => f.IsDeleted == false && f.ProductGroupId == id)
        //        .ToList();
        //    return pgBrands.Select(item => _context.Brands.Find(item.BrandId)).ToList();
        //}

        //public List<Brand> GetBrands()
        //{
        //    return _context.Brands.Where(f => f.IsDeleted == false).ToList();
        //}

        //public List<ProductGroup> GetProductGroups()
        //{
        //    return _context.ProductGroups.Where(f => f.IsDeleted == false).Include(p => p.Children).OrderByDescending(p=>p.InsertDate).ToList();
        //}

        //public ProductGroup AddNewProductGroup(int parentId, string title, List<int> brandIds, List<int> featureIds)
        //{
        //    var productGroup = new ProductGroup();

        //    var user = GetCurrentUser();
        //    productGroup.InsertDate = DateTime.Now;
        //    productGroup.InsertUser = user.UserName;

        //    #region Adding Product Group
        //    productGroup.Title = title;
        //    if (parentId != 0)
        //        productGroup.ParentId = parentId;
        //    _context.ProductGroups.Add(productGroup);
        //    _context.SaveChanges();
        //    _logger.LogEvent(productGroup.GetType().Name, productGroup.Id, "Add");
        //    #endregion

        //    #region Adding Product Group Brands

        //    foreach (var brandId in brandIds)
        //    {
        //        var productGroupBrand = new ProductGroupBrand();
        //        productGroupBrand.ProductGroupId = productGroup.Id;
        //        productGroupBrand.BrandId = brandId;
        //        _context.ProductGroupBrands.Add(productGroupBrand);
        //    }
        //    _context.SaveChanges();

        //    #endregion
        //    #region Adding product Group Features
        //    foreach (var featureId in featureIds)
        //    {
        //        var productGroupFeature = new ProductGroupFeature();
        //        productGroupFeature.ProductGroupId = productGroup.Id;
        //        productGroupFeature.FeatureId = featureId;
        //        _context.ProductGroupFeatures.Add(productGroupFeature);
        //    }

        //    _context.SaveChanges();
        //    #endregion
        //    return productGroup;
        //}
        //public ProductGroup UpdateProductGroup(int parentId,int productGroupId, string title, List<int> brandIds, List<int> featureIds)
        //{
        //    var productGroup = Get(productGroupId);
        //    var user = GetCurrentUser();
        //    productGroup.UpdateDate = DateTime.Now;
        //    productGroup.UpdateUser = user.UserName;

        //    #region Adding Product Group
        //    productGroup.Title = title;
        //    if (parentId != 0)
        //        productGroup.ParentId = parentId;
        //    else
        //        productGroup.ParentId = null;
        //    Update(productGroup);
        //    _logger.LogEvent(productGroup.GetType().Name, productGroup.Id, "Update");
        //    #endregion

        //    #region Product Group Brands

        //    // Removing Previous Group Brands
        //    var productGroupBrands = _context.ProductGroupBrands
        //        .Where(b => b.IsDeleted == false & b.ProductGroupId == productGroup.Id).ToList(); 
        //    foreach (var item in productGroupBrands)
        //    {
        //        item.IsDeleted = true;
        //        _context.Entry(item).State = EntityState.Modified;
        //    }
        //    _context.SaveChanges();
        //    // Adding new Group Brands
        //    foreach (var brandId in brandIds)
        //    {
        //        var productGroupBrand = new ProductGroupBrand();
        //        productGroupBrand.ProductGroupId = productGroup.Id;
        //        productGroupBrand.BrandId = brandId;
        //        _context.ProductGroupBrands.Add(productGroupBrand);
        //    }
        //    _context.SaveChanges();

        //    #endregion
        //    #region product Group Features
        //    var productGroupFeatures = _context.ProductGroupFeatures
        //        .Where(b => b.IsDeleted == false & b.ProductGroupId == productGroup.Id).ToList();
        //    // Removing Previous Group Features
        //    foreach (var item in productGroupFeatures)
        //    {
        //        item.IsDeleted = true;
        //        _context.Entry(item).State = EntityState.Modified;
        //    }
        //    _context.SaveChanges();
        //    // Adding New Group Features
        //    foreach (var featureId in featureIds)
        //    {
        //        var productGroupFeature = new ProductGroupFeature();
        //        productGroupFeature.ProductGroupId = productGroup.Id;
        //        productGroupFeature.FeatureId = featureId;
        //        _context.ProductGroupFeatures.Add(productGroupFeature);
        //    }
        //    _context.SaveChanges();
        //    #endregion
        //    return productGroup;
        //}

        //public List<ProductGroup> GetChildrenProductGroups(int? parentId = null)
        //{
        //    var groups = new List<ProductGroup>();
        //    if (parentId == null)
        //        groups = _context.ProductGroups.Where(p => p.IsDeleted == false && p.ParentId == null).Include(p=>p.Children).ToList();
        //    else
        //        groups = _context.ProductGroups.Where(p => p.IsDeleted == false && p.ParentId == parentId).Include(p => p.Children).ToList();
        //    return groups;
        //}
    }
}
