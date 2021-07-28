using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SPAWedding.Infrastructure.Repositories;
using SPAWedding.Core.Models;
using System.Net;

namespace SPAWedding.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class CourseHeadLinesController : Controller
    {
        private readonly CourseHeadLinesRepository _repo;
        public CourseHeadLinesController(CourseHeadLinesRepository repo)
        {
            _repo = repo;
        }
        public ActionResult Index(int courseId)
        {
            ViewBag.CourseName = _repo.GetCourseName(courseId);
            ViewBag.CourseId = courseId;
            return View(_repo.GetCourseHeadLines(courseId));
        }

        public ActionResult Create(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseHeadLine headLine)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(headLine);
                return RedirectToAction("Index", new { courseId = headLine.CourseId });
            }
            ViewBag.CourseId = headLine.CourseId;
            return View(headLine);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseHeadLine headLine = _repo.Get(id.Value);
            if (headLine == null)
            {
                return HttpNotFound();
            }
            return View(headLine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CourseHeadLine headLine)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(headLine);
                return RedirectToAction("Index", new { courseId = headLine.CourseId });
            }
            return View(headLine);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseHeadLine headLine = _repo.Get(id.Value);
            if (headLine == null)
            {
                return HttpNotFound();
            }
            return PartialView(headLine);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var courseId = _repo.Get(id).CourseId;
            _repo.Delete(id);
            return RedirectToAction("Index", new { courseId });
        }
    }
}