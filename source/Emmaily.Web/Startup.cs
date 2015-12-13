using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Emaily.Web.Startup))]
namespace Emaily.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
