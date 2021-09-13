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

            var allCategories = _context.CourseCategories.Where(c => c.IsDeleted == false).ToList();

            var allMainCategories = allCategories.Where(c => c.ParentId == null).ToList();

            var childs = new List<CourseCategory>();

            foreach (var item in allMainCategories)
            {
                childs = allCategories.Where(c => c.ParentId == item.Id).ToList();

                item.Children = childs;
            }

            return allMainCategories;
        }

        public List<CourseCategory> GetCourseCategoryTable(int id)
        {
            var allCategories = _context.CourseCategories.Where(c => c.ParentId == id && c.IsDeleted == false).ToList();

            foreach (var item in allCategories)
            {
                item.Children = _context.CourseCategories.Where(c => c.IsDeleted == false && c.ParentId == item.Id).ToList();
            }

            return allCategories;
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
            var courseCategory = new CourseCategory();

            var user = GetCurrentUser();
            courseCategory.InsertDate = DateTime.Now;
            courseCategory.InsertUser = user.UserName;

            #region Adding Course Category
            courseCategory.Title = title;
            if (parentId != 0)
                courseCategory.ParentId = parentId;
            _context.CourseCategories.Add(courseCategory);
            _context.SaveChanges();
            _logger.LogEvent(courseCategory.GetType().Name, courseCategory.Id, "Add");
            #endregion

            //#region Adding Course Category Brands

            //foreach (var brandId in brandIds)
            //{
            //    var courseCategoryBrand = new CourseCategoryBrand();
            //    courseCategoryBrand.CourseCategoryId = courseCategory.Id;
            //    courseCategoryBrand.BrandId = brandId;
            //    _context.CourseCategoryBrands.Add(courseCategoryBrand);
            //}
            //_context.SaveChanges();

            //#endregion
            //#region Adding product Group Features
            //foreach (var featureId in featureIds)
            //{
            //    var courseCategoryFeature = new CourseCategoryFeature();
            //    courseCategoryFeature.CourseCategoryId = courseCategory.Id;
            //    courseCategoryFeature.FeatureId = featureId;
            //    _context.CourseCategoryFeatures.Add(courseCategoryFeature);
            //}

            //_context.SaveChanges();
            //#endregion

            return courseCategory;
        }

        public CourseCategory UpdateCourseCategory(int parentId, int courseCategoryId, string title)
        {
            var courseCategory = Get(courseCategoryId);
            var user = GetCurrentUser();
            courseCategory.UpdateDate = DateTime.Now;
            courseCategory.UpdateUser = user.UserName;

            #region Adding Course Category
            courseCategory.Title = title;
            if (parentId != 0)
                courseCategory.ParentId = parentId;
            else
                courseCategory.ParentId = null;
            Update(courseCategory);
            _logger.LogEvent(courseCategory.GetType().Name, courseCategory.Id, "Update");
            #endregion

            //#region Course Category Brands

            //// Removing Previous Group Brands
            //var courseCategoryBrands = _context.CourseCategoryBrands
            //    .Where(b => b.IsDeleted == false & b.CourseCategoryId == courseCategory.Id).ToList();
            //foreach (var item in courseCategoryBrands)
            //{
            //    item.IsDeleted = true;
            //    _context.Entry(item).State = EntityState.Modified;
            //}
            //_context.SaveChanges();
            //// Adding new Group Brands
            //foreach (var brandId in brandIds)
            //{
            //    var courseCategoryBrand = new CourseCategoryBrand();
            //    courseCategoryBrand.CourseCategoryId = courseCategory.Id;
            //    courseCategoryBrand.BrandId = brandId;
            //    _context.CourseCategoryBrands.Add(courseCategoryBrand);
            //}
            //_context.SaveChanges();

            //#endregion

            //#region product Group Features
            //var courseCategoryFeatures = _context.CourseCategoryFeatures
            //    .Where(b => b.IsDeleted == false & b.CourseCategoryId == courseCategory.Id).ToList();
            //// Removing Previous Group Features
            //foreach (var item in courseCategoryFeatures)
            //{
            //    item.IsDeleted = true;
            //    _context.Entry(item).State = EntityState.Modified;
            //}
            //_context.SaveChanges();
            //// Adding New Group Features
            //foreach (var featureId in featureIds)
            //{
            //    var courseCategoryFeature = new CourseCategoryFeature();
            //    courseCategoryFeature.CourseCategoryId = courseCategory.Id;
            //    courseCategoryFeature.FeatureId = featureId;
            //    _context.CourseCategoryFeatures.Add(courseCategoryFeature);
            //}
            //_context.SaveChanges();
            //#endregion

            return courseCategory;
        }

        public List<CourseCategory> GetChildrenCourseCategories(int? parentId = null)
        {
            var allCategories = new List<CourseCategory>();
            if (parentId == null)
                allCategories = _context.CourseCategories.Where(p => p.IsDeleted == false && p.ParentId == null).Include(p => p.Children).ToList();
            else
                allCategories = _context.CourseCategories.Where(p => p.IsDeleted == false && p.ParentId == parentId).Include(p => p.Children).ToList();
            return allCategories;
        }

        public List<CourseCategory> GetAllChildrenCourseCategories()
        {
            var allChildCategories = _context.CourseCategories.Where(p => p.IsDeleted == false && p.ParentId.HasValue).Include(p => p.Children).ToList();

            return allChildCategories;
        }

        public List<CourseCategory> GetMainCourseCategories()
        {
            var allCategories = new List<CourseCategory>();

            allCategories = _context.CourseCategories.Where(p => p.IsDeleted == false && p.ParentId == null).Include(p => p.Children).ToList();

            return allCategories;
        }

        //public CourseCategory GetGroupByProductId(int productId)
        //{
        //    var CourseCategoryId = _context.Products.Where(p => p.IsDeleted == false && p.Id == productId).Select(p => p.CourseCategoryId).FirstOrDefault();

        //    return GetCourseCategory(CourseCategoryId.Value);
        //}
    }
}
