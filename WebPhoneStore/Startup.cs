using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebPhoneStore.Startup))]
namespace WebPhoneStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
