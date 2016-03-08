using System;
using System.Timers;
using System.Web.Hosting;
using Microsoft.Win32;

namespace GasPrice.Api.Timers
{
    internal class IntervalTimer : IRegisteredObject
    {
        internal event EventHandler Elapsed = delegate { };
        private readonly Timer _timer;
        private DateTime _previousRun;
        private int _timeInSeconds;

        internal IntervalTimer(int timeInSeconds = 60)
        {
            HostingEnvironment.RegisterObject(this);
            SystemEvents.TimeChanged += SystemEvents_TimeChanged;

            _timer = new Timer {AutoReset = false};
            _timer.Elapsed += timer_Elapsed;
            _timeInSeconds = timeInSeconds;
        }

        internal void SetInterval(int interval)
        {
            _timeInSeconds = interval;
        }

        internal void Start()
        {
            _previousRun = DateTime.Now;
            var timer = DateTime.Now.AddSeconds(_timeInSeconds);
            var timeSpanToNext = timer.Subtract(DateTime.Now);
            _timer.Interval = timeSpanToNext.TotalMilliseconds;
            _timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_previousRun != DateTime.Now)
                Elapsed(this, EventArgs.Empty);
                
            _timer.Stop();
            Start();
        }

        private void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            _timer.Stop();
            Start();
        }

        public void Stop(bool immediate)
        {
            _timer.Dispose();
            HostingEnvironment.UnregisterObject(this);
        }


    }
}