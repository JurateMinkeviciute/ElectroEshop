using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ElectroEshop.Startup))]
namespace ElectroEshop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
