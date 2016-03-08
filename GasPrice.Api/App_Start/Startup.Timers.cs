using System;
using System.Web.Http;
using GasPrice.Api.Timers;
using GasPrice.Core.Services;

namespace GasPrice.Api
{
    public partial class Startup
    {
        private static IPriceHistoryService _priceHistoryService;

        private static MidnightTimer _sendNotificationTimer;

        private static void SetupTimers()
        {
            var resolver = GlobalConfiguration.Configuration.DependencyResolver;
            _priceHistoryService = (IPriceHistoryService)resolver.GetService(typeof(IPriceHistoryService));

            _sendNotificationTimer = new MidnightTimer(); //new IntervalTimer(5);  
            _sendNotificationTimer.Elapsed += SendNotification;
            _sendNotificationTimer.Start();
        }

        static async void SendNotification(object sender, EventArgs eventArgs)
        {
            await _priceHistoryService.GetAll();
        }

    }
}
