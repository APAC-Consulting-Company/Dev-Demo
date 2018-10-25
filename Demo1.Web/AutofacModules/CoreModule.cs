using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

using Autofac;

namespace Demo1.AutofacModules
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // DAL.AzureAD
            builder.RegisterType<DAL.AzureAD.UserRepository>().AsImplementedInterfaces()
                .WithParameter("clientId", ConfigurationManager.AppSettings["ida:ClientId"])
                .WithParameter("appSecret", ConfigurationManager.AppSettings["ida:ClientSecret"])
                .WithParameter("aadInstance", ConfigurationManager.AppSettings["ida:AADInstance"]);
        }
    }
}