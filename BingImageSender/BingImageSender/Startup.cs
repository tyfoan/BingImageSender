using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BingImageSender.Startup))]
namespace BingImageSender
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
