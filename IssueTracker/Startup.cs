using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IssueTracker.Startup))]
namespace IssueTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
