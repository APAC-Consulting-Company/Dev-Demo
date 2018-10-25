using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Autofac;

namespace Demo1.AutofacModules
{
    public class AutomapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Automapper config
            BL.AutomapperConfig.Configure();
        }
    }
}