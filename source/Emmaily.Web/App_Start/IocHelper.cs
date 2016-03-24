using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Emaily.Core.Abstraction.Services;
using Emaily.Services;
using Emaily.Web.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security.DataProtection;
using Owin;

namespace Emaily.Web
{
    public class IocHelper
    {
        public static IContainer CreateContainer(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule<DataContextModule>();
            builder.RegisterType<ApplicationDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<UtilService>().As<IUtilService>();

            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            GlobalHost.DependencyResolver = new Autofac.Integration.SignalR.AutofacDependencyResolver(container);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());

            return container;
        }
    }
}