using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(The_Masquerade.Startup))]
namespace The_Masquerade
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
