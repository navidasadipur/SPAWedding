using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MaryamRahimiFard.Core.Models;
using MaryamRahimiFard.Infrastructure.Repositories;

namespace MaryamRahimiFard.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class FaqGroupController : Controller
    {
        private readonly FaqGroupsRepository _repo;
        public FaqGroupController(FaqGroupsRepository repo)
        {
            _repo = repo;
        }
        // GET: Admin/Features
        public ActionResult Index()
        {
            return View(_repo.GetAll());
        }

        // GET: Admin/Features/Create
        public ActionResult Create()
        {
            ViewBag.Count = _repo.GetCount();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,OrderPriority")] FaqGroup faqGroup)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(faqGroup);
                return RedirectToAction("Index");
            }
            ViewBag.Count = _repo.GetCount();

            return View(faqGroup);
        }

        // GET: Admin/Features/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FaqGroup faqGroup = _repo.Get(id.Value);
            if (faqGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.Count = _repo.GetCount();
            return PartialView(faqGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,OrderPriority")] FaqGroup faqGroup)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(faqGroup);
                return RedirectToAction("Index");
            }
            ViewBag.Count = _repo.GetCount();
            return View(faqGroup);
        }

        // GET: Admin/Features/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FaqGroup faqGroup = _repo.Get(id.Value);
            if (faqGroup == null)
            {
                return HttpNotFound();
            }
            return PartialView(faqGroup);
        }

        // POST: Admin/Features/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}