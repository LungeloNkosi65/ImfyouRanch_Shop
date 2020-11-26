using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuizeApplication.Startup))]
namespace QuizeApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
