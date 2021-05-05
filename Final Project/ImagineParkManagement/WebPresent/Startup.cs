using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebPresent.Startup))]
namespace WebPresent
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
