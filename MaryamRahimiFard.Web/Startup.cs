using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MaryamRahimiFard.Web.Startup))]
namespace MaryamRahimiFard.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
