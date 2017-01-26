using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinalShoppingCart.Startup))]
namespace FinalShoppingCart
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
