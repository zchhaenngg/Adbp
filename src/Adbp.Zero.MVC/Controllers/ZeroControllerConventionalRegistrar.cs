using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Dependency;
using Castle.MicroKernel.Registration;

namespace Adbp.Zero.MVC.Controllers
{
    /// <summary>
    /// Registers all MVC Controllers derived from <see cref="Controller"/>.
    /// </summary>
    public class ZeroControllerConventionalRegistrar : IConventionalDependencyRegistrar
    {
        /// <inheritdoc/>
        public void RegisterAssembly(IConventionalRegistrationContext context)
        {
            context.IocManager.IocContainer.Register(
                Classes.FromAssembly(Assembly.GetCallingAssembly())
                    .BasedOn<Controller>()
                    .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                    .LifestyleTransient()
                );
        }
    }
}
