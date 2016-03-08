using GasPrice.Api;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace GasPrice.Api
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            var container = ConfigIoC(app);

            app.UseCors(CorsOptions.AllowAll);

            SetupTimers();

            ConfigOAuth(app);
            ConfigWebApi(app);  
            container.Verify();
        }

    }
}
