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
using SPAWedding.Infrastructure;
using SPAWedding.Infrastructure.Helpers;
using SPAWedding.Infrastructure.Repositories;
using SPAWedding.Web.ViewModels;

namespace SPAWedding.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly CoursesRepository _repo;
        public CoursesController(CoursesRepository repo)
        {
            _repo = repo;
        }
        // GET: Admin/Courses
        public ActionResult Index()
        {
            var courses = _repo.GetCourses();
            var coursesListVm = new List<CourseInfoViewModel>();
            foreach (var course in courses)
            {
                var courseVm = new CourseInfoViewModel(course);
                coursesListVm.Add(courseVm);
            }
            return View(coursesListVm);
        }
        // GET: Admin/Courses/Create
        public ActionResult Create()
        {
            ViewBag.CourseCategoryId = new SelectList(_repo.GetCourseCategories(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course, HttpPostedFileBase CourseImage, string Tags)
        {
            if (ModelState.IsValid)
            {

                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    ViewBag.Message = "کاربر وارد کننده پیدا نشد.";
                    return View(course);
                }


                #region Upload Image
                if (CourseImage != null)
                {
                    // Saving Temp Image
                    var newFileName = Guid.NewGuid() + Path.GetExtension(CourseImage.FileName);
                    CourseImage.SaveAs(Server.MapPath("/Files/CourseImages/Temp/" + newFileName));

                    // Resize Image
                    ImageResizer image = new ImageResizer(1200, 1200, true);
                    image.Resize(Server.MapPath("/Files/CourseImages/Temp/" + newFileName),
                        Server.MapPath("/Files/CourseImages/Image/" + newFileName));

                    ImageResizer thumb = new ImageResizer(300, 300, true);
                    thumb.Resize(Server.MapPath("/Files/CourseImages/Temp/" + newFileName),
                        Server.MapPath("/Files/CourseImages/Thumb/" + newFileName));

                    // Deleting Temp Image
                    System.IO.File.Delete(Server.MapPath("/Files/CourseImages/Temp/" + newFileName));

                    course.Image = newFileName;
                }
                #endregion

                _repo.AddCourse(course);

                //if (!string.IsNullOrEmpty(Tags))
                //    _repo.AddCourseTags(course.Id, Tags);

                return RedirectToAction("Index");
            }
            ViewBag.Tags = Tags;
            ViewBag.CourseCategoryId = new SelectList(_repo.GetCourseCategories(), "Id", "Title", course.CourseCategoryId);
            return View(course);
        }

        // GET: Admin/Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = _repo.GetCourse(id.Value);
            if (course == null)
            {
                return HttpNotFound();
            }

            //ViewBag.Tags = _repo.GetCourseTagsStr(id.Value);

            ViewBag.CourseCategoryId = new SelectList(_repo.GetCourseCategories(), "Id", "Title", course.CourseCategoryId);
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course, HttpPostedFileBase CourseImage, string Tags)
        {
            if (ModelState.IsValid)
            {
                #region Upload Image
                if (CourseImage != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("/Files/CourseImages/Image/" + course.Image)))
                        System.IO.File.Delete(Server.MapPath("/Files/CourseImages/Image/" + course.Image));

                    if (System.IO.File.Exists(Server.MapPath("/Files/CourseImages/Thumb/" + course.Image)))
                        System.IO.File.Delete(Server.MapPath("/Files/CourseImages/Thumb/" + course.Image));

                    // Saving Temp Image
                    var newFileName = Guid.NewGuid() + Path.GetExtension(CourseImage.FileName);
                    CourseImage.SaveAs(Server.MapPath("/Files/CourseImages/Temp/" + newFileName));

                    // Resize Image
                    ImageResizer image = new ImageResizer(1200, 1200, true);
                    image.Resize(Server.MapPath("/Files/CourseImages/Temp/" + newFileName),
                        Server.MapPath("/Files/CourseImages/Image/" + newFileName));

                    ImageResizer thumb = new ImageResizer(300, 300, true);
                    thumb.Resize(Server.MapPath("/Files/CourseImages/Temp/" + newFileName),
                        Server.MapPath("/Files/CourseImages/Thumb/" + newFileName));

                    // Deleting Temp Image
                    System.IO.File.Delete(Server.MapPath("/Files/CourseImages/Temp/" + newFileName));

                    course.Image = newFileName;
                }
                #endregion

                _repo.Update(course);

                //if (!string.IsNullOrEmpty(Tags))
                //    _repo.AddCourseTags(course.Id, Tags);

                return RedirectToAction("Index");
            }
            ViewBag.Tags = Tags;
            ViewBag.CourseCategoryId = new SelectList(_repo.GetCourseCategories(), "Id", "Title", course.CourseCategoryId);
            return View(course);
        }

        [HttpPost]
        public ActionResult FileUpload()
        {
            var files = HttpContext.Request.Files;
            foreach (var fileName in files)
            {
                HttpPostedFileBase file = Request.Files[fileName.ToString()];
                var newFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("/Files/CourseImages/" + newFileName));
                TempData["UploadedFile"] = newFileName;
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        // GET: Admin/Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = _repo.GetCourse(id.Value);
            if (course == null)
            {
                return HttpNotFound();
            }
            return PartialView(course);
        }

        // POST: Admin/Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var course = _repo.Get(id);

            //#region Delete Course Image
            //if (course.Image != null)
            //{
            //    if (System.IO.File.Exists(Server.MapPath("/Files/CourseImages/Image/" + course.Image)))
            //        System.IO.File.Delete(Server.MapPath("/Files/CourseImages/Image/" + course.Image));

            //    if (System.IO.File.Exists(Server.MapPath("/Files/CourseImages/Thumb/" + course.Image)))
            //        System.IO.File.Delete(Server.MapPath("/Files/CourseImages/Thumb/" + course.Image));
            //}
            //#endregion

            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
