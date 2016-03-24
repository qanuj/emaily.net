using System.Data.Entity;
using Autofac;
using Emaily.Web.Models;

namespace Emaily.Web
{
    public class DataContextModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
        }
    }
}