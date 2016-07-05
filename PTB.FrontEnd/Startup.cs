using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PTB.FrontEnd.Startup))]
namespace PTB.FrontEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
