using System.Web.Mvc;
using MaryamRahimiFard.Infrastructure.Repositories;
using MaryamRahimiFard.Web.Areas.Customer.Controllers;
using MaryamRahimiFard.Web.Controllers;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace MaryamRahimiFard.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<AuthController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());
            container.RegisterType<ArticleCategoriesRepository>();
            container.RegisterType<ArticlesRepository>();
            container.RegisterType<UsersRepository>();
            //container.RegisterType<UsersController>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}