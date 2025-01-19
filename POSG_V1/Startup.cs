using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(POSG_V1.Startup))]
namespace POSG_V1
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
