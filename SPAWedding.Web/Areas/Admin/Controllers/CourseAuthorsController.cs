using System;
using System.Net;
using System.Web.Mvc;
using MaryamRahimiFard.Core.Models;
using MaryamRahimiFard.Infrastructure.Repositories;

namespace MaryamRahimiFard.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class CourseAuthorsController : Controller
    {
        private readonly CourseAuthorsRepository _repo;
        public CourseAuthorsController(CourseAuthorsRepository repo)
        {
            _repo = repo;
        }
        // GET: Admin/CourseAuthors
        public ActionResult Index()
        {
            return View(_repo.GetAll());
        }

        // GET: Admin/CourseAuthors/Create
        public ActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Filed,AboutAuthor,MoreInfo")] CourseAuthor courseAuthor)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(courseAuthor);
                return RedirectToAction("Index");
            }

            return View(courseAuthor);
        }

        // GET: Admin/CourseAuthors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAuthor courseAuthor = _repo.Get(id.Value);
            if (courseAuthor == null)
            {
                return HttpNotFound();
            }
            return PartialView(courseAuthor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Filed,AboutAuthor,MoreInfo")] CourseAuthor courseAuthor)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(courseAuthor);
                return RedirectToAction("Index");
            }
            return View(courseAuthor);
        }

        // GET: Admin/CourseAuthors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAuthor courseAuthor = _repo.Get(id.Value);
            if (courseAuthor == null)
            {
                return HttpNotFound();
            }
            return PartialView(courseAuthor);
        }

        // POST: Admin/CourseAuthors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
