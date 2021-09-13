using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MaryamRahimiFard.Core.Models;
using MaryamRahimiFard.Infrastructure.Repositories;
using MaryamRahimiFard.Web.ViewModels;

namespace MaryamRahimiFard.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class CourseCategoriesController : Controller
    {
        private readonly CourseCategoriesRepository _repo;
        public CourseCategoriesController(CourseCategoriesRepository repo)
        {
            _repo = repo;
        }

        // GET: Admin/CourseCategories
        public ActionResult Index(int? parentId)
        {
            List<CourseCategory> CourseCategories;
            if (parentId == null)
                CourseCategories = _repo.GetCourseCategoryTable();
            else
            {
                CourseCategories = _repo.GetCourseCategoryTable(parentId.Value);
                var parent = _repo.Get(parentId.Value);
                ViewBag.PrevParent = parent.ParentId;
                ViewBag.ParentId = parentId;
                ViewBag.ParentName = parent.Title;
            }
            return View(CourseCategories);
        }
        // GET: Admin/CourseCategories/Create
        public ActionResult Create()
        {
            ViewBag.CourseCategories = _repo.GetCourseCategories();
            return View();
        }

        [HttpPost]
        public int? Create(NewCourseCategoryViewModel courseCategory)
        {
            if (ModelState.IsValid)
            {
                var product = _repo.AddNewCourseCategory(courseCategory.ParentCategoryId, courseCategory.Title);
                return product.Id;
            }

            return null;
        }
        // GET: Admin/CourseCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCategory CourseCategory = _repo.GetCourseCategory(id.Value);
            if (CourseCategory == null)
            {
                return HttpNotFound();
            }

            ViewBag.CourseCategories = _repo.GetCourseCategories();

            return View(CourseCategory);
        }

        [HttpPost]
        public int? Edit(UpdateCourseCategoryViewModel CourseCategory)
        {
            if (ModelState.IsValid)
            {
                var product = _repo.UpdateCourseCategory(CourseCategory.ParentCategoryId, CourseCategory.Id, CourseCategory.Title);
                return product.Id;
            }

            return null;
        }
        // GET: Admin/CourseCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCategory CourseCategory = _repo.Get(id.Value);
            if (CourseCategory == null)
            {
                return HttpNotFound();
            }
            return PartialView(CourseCategory);
        }

        //[HttpPost]
        //public bool UploadImage(int id, HttpPostedFileBase File)
        //{
        //    #region Upload Image
        //    if (File != null)
        //    {
        //        var CourseCategory = _repo.Get(id);
        //        if (CourseCategory.Image != null)
        //        {
        //            if (System.IO.File.Exists(Server.MapPath("/Files/CourseCategoryImages/Image/" + CourseCategory.Image)))
        //                System.IO.File.Delete(Server.MapPath("/Files/CourseCategoryImages/Image/" + CourseCategory.Image));

        //            if (System.IO.File.Exists(Server.MapPath("/Files/CourseCategoryImages/Thumb/" + CourseCategory.Image)))
        //                System.IO.File.Delete(Server.MapPath("/Files/CourseCategoryImages/Thumb/" + CourseCategory.Image));
        //        }
        //        // Saving Temp Image
        //        var newFileName = Guid.NewGuid() + Path.GetExtension(File.FileName);
        //        File.SaveAs(Server.MapPath("/Files/CourseCategoryImages/Temp/" + newFileName));
        //        // Resize Image
        //        ImageResizer image = new ImageResizer(850, 400, true);
        //        image.Resize(Server.MapPath("/Files/CourseCategoryImages/Temp/" + newFileName),
        //            Server.MapPath("/Files/CourseCategoryImages/Image/" + newFileName));

        //        ImageResizer thumb = new ImageResizer(200, 200, true);
        //        thumb.Resize(Server.MapPath("/Files/CourseCategoryImages/Temp/" + newFileName),
        //            Server.MapPath("/Files/CourseCategoryImages/Thumb/" + newFileName));

        //        // Deleting Temp Image
        //        System.IO.File.Delete(Server.MapPath("/Files/CourseCategoryImages/Temp/" + newFileName));
        //        CourseCategory.Image = newFileName;
        //        _repo.Update(CourseCategory);
        //        return true;
        //    }
        //    #endregion
        //    return false;
        //}

        // POST: Admin/CourseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var CourseCategory = _repo.Get(id);
            var parentId = CourseCategory.ParentId;
            //#region Delete CourseCategory Image
            //if (CourseCategory.Image != null)
            //{
            //    if (System.IO.File.Exists(Server.MapPath("/Files/CourseCategoryImages/Image/" + CourseCategory.Image)))
            //        System.IO.File.Delete(Server.MapPath("/Files/CourseCategoryImages/Image/" + CourseCategory.Image));

            //    if (System.IO.File.Exists(Server.MapPath("/Files/CourseCategoryImages/Thumb/" + CourseCategory.Image)))
            //        System.IO.File.Delete(Server.MapPath("/Files/CourseCategoryImages/Thumb/" + CourseCategory.Image));
            //}
            //#endregion

            _repo.Delete(id);
            return RedirectToAction("Index", new { parentId });
        }
    }
}
