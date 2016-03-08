using System;
using System.Timers;
using System.Web.Hosting;
using Microsoft.Win32;

namespace GasPrice.Api.Timers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Hosting.IRegisteredObject" />
    internal class MidnightTimer : IRegisteredObject
    {
        internal event EventHandler Elapsed = delegate { };
        private readonly Timer _timer;
        private DateTime _previousRun;

        internal MidnightTimer()
        {
            HostingEnvironment.RegisterObject(this);
            SystemEvents.TimeChanged += SystemEvents_TimeChanged;

            _timer = new Timer { AutoReset = false };
            _timer.Elapsed += timer_Elapsed;
        }

        internal void Start()
        {
            _previousRun = DateTime.Today;

            var timeSpanToMidnight = GetNextMidnight().Subtract(DateTime.Now);
            _timer.Interval = timeSpanToMidnight.TotalMilliseconds;
            _timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_previousRun != DateTime.Today)
                Elapsed(this, EventArgs.Empty);

            _timer.Stop();
            Start();
        }

        private void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            _timer.Stop();
            Start();
        }

        private static DateTime GetNextMidnight()
        {
            return DateTime.Today.AddDays(1);
        }

        public void Stop(bool immediate)
        {
            _timer.Dispose();
            HostingEnvironment.UnregisterObject(this);
        }
    }
}