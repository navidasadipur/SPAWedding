using SPAWedding.Infratructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPAWedding.Web.Areas.Admin.Controllers
{
    public class EmailSubscriptionController : Controller
    {
        private readonly EmailSubscriptionRepository _emailSubscriptionRepo;

        public EmailSubscriptionController(EmailSubscriptionRepository emailSubscriptionRepository)
        {
            _emailSubscriptionRepo = emailSubscriptionRepository;
        }

        // GET: Admin/EmailSubscription
        public ActionResult Index()
        {
            var emailSubscriptions = _emailSubscriptionRepo.GetAll();
            return View(emailSubscriptions);
        }

        public ActionResult Delete(int? id)
        {
            var emailSubscription = _emailSubscriptionRepo.Get(id.Value);
            return PartialView(emailSubscription);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _emailSubscriptionRepo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}