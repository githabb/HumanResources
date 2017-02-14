using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HumanResources.Web.Startup))]
namespace HumanResources.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
