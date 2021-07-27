using SPAWedding.Infrastructure;
using SPAWedding.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SPAWedding.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly EPaymentRepository _ePaymentRepo;
        public MvcApplication()
        {
            MyDbContext _dbcontext = new MyDbContext();
            _ePaymentRepo = new EPaymentRepository(_dbcontext, new LogsRepository(_dbcontext));
        }

        protected void Application_Start()
        {
            Thread cronJobs = new Thread(CronJobs);
            cronJobs.IsBackground = true;
            cronJobs.Start();

            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Thread cronJobs = (Thread)Application["CronJobs"];
                if (cronJobs != null && cronJobs.IsAlive)
                {
                    cronJobs.Abort();
                }
            }
        }

        public void CronJobs()
        {
            while (true)
            {
                // we need to expire all payments that 10 mintues has passed from initiating them
                _ePaymentRepo.ExpireEPayments();

                Thread.Sleep(60000);
            }
        }

    }
}
