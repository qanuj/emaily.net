using System.Web;
using Autofac;
using Emaily.Core.Abstraction.Persistence;
using Emaily.Data;
using Emaily.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;

namespace Emaily.Web
{
    public class DataContextModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationRoleStore>().AsSelf().As<RoleStore<Role>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().AsSelf().As<UserStore<User>>().InstancePerRequest();

            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerRequest();

            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();

            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerRequest();
        }
    }
}