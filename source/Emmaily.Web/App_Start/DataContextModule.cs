using System.Data.Entity;
using Autofac;
using Emaily.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;

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
        }
    }
}