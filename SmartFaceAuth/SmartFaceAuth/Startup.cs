using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartFaceAuth.Startup))]
namespace SmartFaceAuth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
