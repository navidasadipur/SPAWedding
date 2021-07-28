//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using SPAWedding.Infrastructure.Repositories;
//using SPAWedding.Core.Models;
//using System.Net;
//using SPAWedding.Web.ViewModels;

//namespace SPAWedding.Web.Areas.Admin.Controllers
//{
//    [Authorize]
//    public class CourseCommentsController : Controller
//    {
//        private readonly CourseCommentsRepository _repo;
//        public CourseCommentsController(CourseCommentsRepository repo)
//        {
//            _repo = repo;
//        }
//        public ActionResult Index(int CourseId)
//        {
//            ViewBag.CourseName = _repo.GetCourseName(CourseId);
//            ViewBag.CourseId = CourseId;
//            var comments = _repo.GetCourseComments(CourseId);
//            var commentsVm = new List<CommentWithPersianDateViewModel>();
//            foreach (var comment in comments)
//            {
//                var commentVm = new CommentWithPersianDateViewModel(comment);
//                commentsVm.Add(commentVm);
//            }
//            return View(commentsVm);
//        }

//        public ActionResult Create(int CourseId)
//        {
//            ViewBag.CourseId = CourseId;
//            return View();
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(CourseComment comment)
//        {
//            if (ModelState.IsValid)
//            {
//                comment.AddedDate = DateTime.Now;
//                _repo.Add(comment);
//                return RedirectToAction("Index", new { CourseId = comment.CourseId });
//            }
//            ViewBag.CourseId = comment.CourseId;
//            return View(comment);
//        }
//        public ActionResult AnswerComment(int CourseId,int parentCommentId)
//        {
//            ViewBag.CourseId = CourseId;
//            ViewBag.ParentId = parentCommentId;
//            return PartialView();
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult AnswerComment(CourseComment comment)
//        {
//            var user = _repo.GetCurrentUser();
//            comment.Name = user != null? $"{user.FirstName} {user.LastName}" : "ادمین";
//            comment.Email = user != null ? user.Email :"-";
//            comment.AddedDate = DateTime.Now;
//            _repo.Add(comment);
//            return RedirectToAction("Index", new { CourseId = comment.CourseId });
//        }
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CourseComment comment = _repo.Get(id.Value);
//            if (comment == null)
//            {
//                return HttpNotFound();
//            }
//            return View(comment);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(CourseComment comment)
//        {
//            if (ModelState.IsValid)
//            {
//                _repo.Update(comment);
//                return RedirectToAction("Index", new { CourseId = comment.CourseId });
//            }
//            return View(comment);
//        }

//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CourseComment comment = _repo.Get(id.Value);
//            if (comment == null)
//            {
//                return HttpNotFound();
//            }

//            var commentVm = new CommentWithPersianDateViewModel(comment);
//            return PartialView(commentVm);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            var CourseId = _repo.Get(id).CourseId;
//            _repo.DeleteComment(id);
//            return RedirectToAction("Index", new { CourseId });
//        }
//    }
//}