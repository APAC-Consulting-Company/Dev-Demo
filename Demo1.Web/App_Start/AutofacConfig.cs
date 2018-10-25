using System;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;

namespace Demo1.App_Start
{
    public static class AutofacConfig
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // Register application's modules.
            builder.RegisterModule<AutofacModules.MediatrModule>();
            builder.RegisterModule<AutofacModules.AutomapperModule>();
            builder.RegisterModule<AutofacModules.CoreModule>();

            var container = builder.Build();

            return container;
        }
    }
}