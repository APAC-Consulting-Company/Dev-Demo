using System;
using System.Threading.Tasks;
using Demo1.App_Start;
using Microsoft.Owin;
using Owin;

namespace Demo1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AutofacBootstrapper.UseAutofac(app);
            ConfigureAuth(app);
        }
    }
}
