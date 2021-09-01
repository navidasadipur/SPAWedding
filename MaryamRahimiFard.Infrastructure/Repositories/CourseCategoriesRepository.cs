using MaryamRahimiFard.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaryamRahimiFard.Infrastructure.Repositories
{
    public class CourseCategoriesRepository : BaseRepository<CourseCategory, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public CourseCategoriesRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<CourseCategory> GetCourseCategoryTable()
        {
            var allGroups = _context.CourseCategories.Where(c => c.ParentId == null && c.IsDeleted == false).Include(c => c.Children).ToList();

            var removableListIds = new List<int>();

            foreach (var item in allGroups)
            {
                foreach (var child in item.Children)
                {
                    if (child.IsDeleted == true)
                        removableListIds.Add(child.Id);
                }
            }

            allGroups.RemoveAll(g => removableListIds.Contains(g.Id));

            return allGroups;
        }
        public List<CourseCategory> GetCourseCategoryTable(int id)
        {
            var allGroups = _context.CourseCategories.Where(c => c.ParentId == id && c.IsDeleted == false).Include(c => c.Children).ToList();

            var removableListIds = new List<int>();

            var removeableChild = new CourseCategory();

            foreach (var item in allGroups)
            {
                foreach (var child in item.Children)
                {
                    if (child.IsDeleted == true)
                        removeableChild = child;
                }

                item.Children.Remove(removeableChild);
            }

            allGroups.RemoveAll(g => removableListIds.Contains(g.Id));

            return allGroups;
        }
        public CourseCategory GetCourseCategory(int id)
        {
            var cc = _context.CourseCategories.FirstOrDefault(c => c.Id == id);
            return cc;
        }
        //public List<Feature> GetFeatures()
        //{
        //    return _context.Features.Where(f => f.IsDeleted == false).ToList();
        //}
        //public List<Feature> GetCourseCategoryFeatures(int id)
        //{
        //    var pgFeatures = _context.CourseCategoryFeatures.Where(f => f.IsDeleted == false && f.CourseCategoryId == id)
        //        .ToList();
        //    return pgFeatures.Select(item => _context.Features.Find(item.FeatureId)).ToList();
        //}
        //public List<Brand> GetCourseCategoryBrands(int id)
        //{
        //    var pgBrands = _context.CourseCategoryBrands.Where(f => f.IsDeleted == false && f.CourseCategoryId == id)
        //        .ToList();
        //    return pgBrands.Select(item => _context.Brands.Find(item.BrandId)).ToList();
        //}
        //public List<Brand> GetBrands()
        //{
        //    return _context.Brands.Where(f => f.IsDeleted == false).ToList();
        //}

        public List<CourseCategory> GetCourseCategories()
        {
            return _context.CourseCategories.Where(f => f.IsDeleted == false).Include(p => p.Children).OrderByDescending(p => p.InsertDate).ToList();
        }

        public CourseCategory AddNewCourseCategory(int parentId, string title)
        {
            var productGroup = new CourseCategory();

            var user = GetCurrentUser();
            productGroup.InsertDate = DateTime.Now;
            productGroup.InsertUser = user.UserName;

            #region Adding Product Group
            productGroup.Title = title;
            if (parentId != 0)
                productGroup.ParentId = parentId;
            _context.CourseCategories.Add(productGroup);
            _context.SaveChanges();
            _logger.LogEvent(productGroup.GetType().Name, productGroup.Id, "Add");
            #endregion

            //#region Adding Product Group Brands

            //foreach (var brandId in brandIds)
            //{
            //    var productGroupBrand = new CourseCategoryBrand();
            //    productGroupBrand.CourseCategoryId = productGroup.Id;
            //    productGroupBrand.BrandId = brandId;
            //    _context.CourseCategoryBrands.Add(productGroupBrand);
            //}
            //_context.SaveChanges();

            //#endregion
            //#region Adding product Group Features
            //foreach (var featureId in featureIds)
            //{
            //    var productGroupFeature = new CourseCategoryFeature();
            //    productGroupFeature.CourseCategoryId = productGroup.Id;
            //    productGroupFeature.FeatureId = featureId;
            //    _context.CourseCategoryFeatures.Add(productGroupFeature);
            //}

            //_context.SaveChanges();
            //#endregion

            return productGroup;
        }

        public CourseCategory UpdateCourseCategory(int parentId, int productGroupId, string title)
        {
            var productGroup = Get(productGroupId);
            var user = GetCurrentUser();
            productGroup.UpdateDate = DateTime.Now;
            productGroup.UpdateUser = user.UserName;

            #region Adding Product Group
            productGroup.Title = title;
            if (parentId != 0)
                productGroup.ParentId = parentId;
            else
                productGroup.ParentId = null;
            Update(productGroup);
            _logger.LogEvent(productGroup.GetType().Name, productGroup.Id, "Update");
            #endregion

            //#region Product Group Brands

            //// Removing Previous Group Brands
            //var productGroupBrands = _context.CourseCategoryBrands
            //    .Where(b => b.IsDeleted == false & b.CourseCategoryId == productGroup.Id).ToList();
            //foreach (var item in productGroupBrands)
            //{
            //    item.IsDeleted = true;
            //    _context.Entry(item).State = EntityState.Modified;
            //}
            //_context.SaveChanges();
            //// Adding new Group Brands
            //foreach (var brandId in brandIds)
            //{
            //    var productGroupBrand = new CourseCategoryBrand();
            //    productGroupBrand.CourseCategoryId = productGroup.Id;
            //    productGroupBrand.BrandId = brandId;
            //    _context.CourseCategoryBrands.Add(productGroupBrand);
            //}
            //_context.SaveChanges();

            //#endregion

            //#region product Group Features
            //var productGroupFeatures = _context.CourseCategoryFeatures
            //    .Where(b => b.IsDeleted == false & b.CourseCategoryId == productGroup.Id).ToList();
            //// Removing Previous Group Features
            //foreach (var item in productGroupFeatures)
            //{
            //    item.IsDeleted = true;
            //    _context.Entry(item).State = EntityState.Modified;
            //}
            //_context.SaveChanges();
            //// Adding New Group Features
            //foreach (var featureId in featureIds)
            //{
            //    var productGroupFeature = new CourseCategoryFeature();
            //    productGroupFeature.CourseCategoryId = productGroup.Id;
            //    productGroupFeature.FeatureId = featureId;
            //    _context.CourseCategoryFeatures.Add(productGroupFeature);
            //}
            //_context.SaveChanges();
            //#endregion

            return productGroup;
        }

        public List<CourseCategory> GetChildrenCourseCategories(int? parentId = null)
        {
            var groups = new List<CourseCategory>();
            if (parentId == null)
                groups = _context.CourseCategories.Where(p => p.IsDeleted == false && p.ParentId == null).Include(p => p.Children).ToList();
            else
                groups = _context.CourseCategories.Where(p => p.IsDeleted == false && p.ParentId == parentId).Include(p => p.Children).ToList();
            return groups;
        }

        public List<CourseCategory> GetMainCourseCategories()
        {
            var groups = new List<CourseCategory>();

            groups = _context.CourseCategories.Where(p => p.IsDeleted == false && p.ParentId == null).Include(p => p.Children).ToList();

            return groups;
        }

        //public CourseCategory GetGroupByProductId(int productId)
        //{
        //    var CourseCategoryId = _context.Products.Where(p => p.IsDeleted == false && p.Id == productId).Select(p => p.CourseCategoryId).FirstOrDefault();

        //    return GetCourseCategory(CourseCategoryId.Value);
        //}
    }
}
