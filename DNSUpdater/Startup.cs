using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DNSUpdater.Startup))]
namespace DNSUpdater
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
