using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartupAttribute(typeof(Emaily.Web.Startup))]
namespace Emaily.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            IocHelper.CreateContainer(app);
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
