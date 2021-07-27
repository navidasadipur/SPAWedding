using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SPAWedding.Web.Startup))]
namespace SPAWedding.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
