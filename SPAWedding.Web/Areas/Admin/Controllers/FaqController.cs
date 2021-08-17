using System;
using System.Net;
using System.Web.Mvc;
using MaryamRahimiFard.Core.Models;
using MaryamRahimiFard.Infrastructure.Repositories;

namespace MaryamRahimiFard.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class FaqController : Controller
    {
        private readonly FaqRepository _faqRepo;
        private readonly FaqGroupsRepository _faqGroupsRepo;
        public FaqController(
            FaqRepository faqRepo,
            FaqGroupsRepository FaqGroupsRepo)
        {
            _faqRepo = faqRepo;
            _faqGroupsRepo = FaqGroupsRepo;
        }
        public ActionResult Index(int faqGroupId)
        {
            ViewBag.GroupTitle = _faqGroupsRepo.GetFaqGroupTitle(faqGroupId);
            ViewBag.FaqGroupId = faqGroupId;

            var allFaqs = _faqGroupsRepo.GetAllFaqbyFaqGroupId(faqGroupId);

            return View(allFaqs);
        }
        public ActionResult Create(int test)
        {
            ViewBag.FaqGroupId = test;
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Faq faq)
        {
            if (ModelState.IsValid)
            {
                _faqRepo.Add(faq);
                return RedirectToAction("Index", new { faqGroupId = faq.FaqGroupId });
            }

            return View(faq);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faq faq = _faqRepo.Get(id.Value);
            if (faq == null)
            {
                return HttpNotFound();
            }

            ViewBag.FaqGroupId = faq.FaqGroupId;
            return PartialView(faq);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Faq faq)
        {
            if (ModelState.IsValid)
            {
                _faqRepo.Update(faq);
                return RedirectToAction("Index", new { faqGroupId = faq.FaqGroupId });
            }
            return View(faq);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faq faq = _faqRepo.Get(id.Value);
            if (faq == null)
            {
                return HttpNotFound();
            }
            return PartialView(faq);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var faqGroupId = _faqRepo.Get(id).FaqGroupId;
            _faqRepo.Delete(id);
            return RedirectToAction("Index", new { faqGroupId = faqGroupId });
        }
    }
}