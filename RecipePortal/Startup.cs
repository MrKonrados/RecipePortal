using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RecipePortal.Startup))]
namespace RecipePortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
