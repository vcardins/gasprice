using System.Web;
using System.Web.Http;
using GasPrice.Api.Extensions;
using GasPrice.Api.Helpers;
using GasPrice.Config;
using GasPrice.Core.Account;
using GasPrice.Core.Common.Information;
using GasPrice.Core.Common.Infrastructure;
using GasPrice.Core.Common.Infrastructure.ImageHandler.Interfaces;
using GasPrice.Core.Common.Messaging.Interfaces;
using GasPrice.Core.Config.Security;
using GasPrice.Core.Data.DataContext;
using GasPrice.Core.Data.Repositories;
using GasPrice.Core.Data.UnitOfWork;
using GasPrice.Core.Services;
using GasPrice.Data.EF6;
using GasPrice.Infrastructure.Caching;
using GasPrice.Infrastructure.Crypto;
using GasPrice.Infrastructure.Messaging;
using GasPrice.Services.Account;
using GasPrice.Services.Services;
using Owin;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web;

namespace GasPrice.Api
{
    public partial class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="System.Web.HttpContextBase" />
        public class FakeHttpContext : HttpContextBase { }

        /// <summary>
        /// Starts the application
        /// </summary>
        public static Container ConfigIoC(IAppBuilder appBuilder)
        {
            var container = new Container();
            RegisterDependencies(appBuilder, container);
            return container;
        }

        /// <summary>
        /// Registers the dependencies.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="container">The container.</param>
        public static void RegisterDependencies(IAppBuilder app, Container container)
        {

            const string nameOrConnectionString = "name=DataContext";

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Register(typeof(IRepositoryAsync<>), typeof(Repository<>), Lifestyle.Scoped);

            container.Register(typeof(IUnitOfWorkAsync), typeof(UnitOfWork), Lifestyle.Scoped);

            container.RegisterWithContext<IDataContextAsync>(c => new AppDataContext(nameOrConnectionString));

            container.RegisterPerWebRequest<HttpContextBase>(() =>
            {
                var context = HttpContext.Current;
                if (context == null && container.IsVerifying()) return new FakeHttpContext();
                return context !=null ? new HttpContextWrapper(context) : null;
            });

            container.Register<IUserAccountService, UserAccountService>();
            container.Register<IAppSettingsService, AppSettingsService>();
            container.Register<ILookupService, LookupService>();
            container.Register<IUserService, UserService>();
            container.Register<IUserProfileService, UserProfileService>();
            container.Register<IExceptionLogService, ExceptionLogService>();
            container.Register<IPriceHistoryService, PriceHistoryService>();
            
            container.RegisterSingleton(AppConfig.Instance.Messaging.Mailer.Current);
            container.RegisterSingleton<ISecuritySettings>(AppConfig.Instance.Security);
            container.RegisterSingleton<IImageSettings>(AppConfig.Instance.Images);
            container.RegisterSingleton<IApplicationInformation>(AppConfig.Instance.Information);
            container.RegisterSingleton<ICrypto, DefaultCrypto>();
            container.RegisterSingleton<IEmailService, SmtpMessageDelivery>();
            container.RegisterSingleton<IDataCache, MyCache>(); 
            

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            
        }
    }
}