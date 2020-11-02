using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImfuyoRanch.Startup))]
namespace ImfuyoRanch
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
