using Autofac.Integration.Mvc;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo1.App_Start
{
    public static class AutofacBootstrapper
    {
        public static Autofac.IContainer UseAutofac(IAppBuilder app)
        {
            var container = AutofacConfig.CreateContainer();
            
            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return container;
        }

    }
}