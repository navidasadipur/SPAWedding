using System;
using System.Net;
using System.Web.Mvc;
using MaryamRahimiFard.Core.Models;
using MaryamRahimiFard.Infrastructure.Repositories;

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
        public ActionResult Index()
        {
            return View(_repo.GetAll());
        }

        // GET: Admin/CourseCategories/Create
        public ActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title")] CourseCategory courseCategory)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(courseCategory);
                return RedirectToAction("Index");
            }

            return View(courseCategory);
        }

        // GET: Admin/CourseCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCategory courseCategory = _repo.Get(id.Value);
            if (courseCategory == null)
            {
                return HttpNotFound();
            }
            return PartialView(courseCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] CourseCategory courseCategory)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(courseCategory);
                return RedirectToAction("Index");
            }
            return View(courseCategory);
        }

        // GET: Admin/CourseCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCategory courseCategory = _repo.Get(id.Value);
            if (courseCategory == null)
            {
                return HttpNotFound();
            }
            return PartialView(courseCategory);
        }

        // POST: Admin/CourseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
