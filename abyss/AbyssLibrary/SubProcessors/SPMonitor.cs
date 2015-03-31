using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class SPMonitor : AbstractSubProcessor
    {
        bool startCountdown = false;

        [AbyssParameter]
        public List<AbyssMonitorController> Monitors
        {
            get;
            set;
        }

        [AbyssOutput]
        public event AbyssEvent CountdownExpired;

        [AbyssOutput]
        public event AbyssEvent CountdownStopped;

        [AbyssOutput]
        public event AbyssEvent CountdownStarted;

        [AbyssInput]
        public void Start(object sender, EventArgs e)
        {
            startCountdown = true;
            StartProcess();
        }

        [AbyssInput]
        public void Stop(object sender, EventArgs e)
        {
            startCountdown = false;
            StartProcess();
        }

        protected override void Process()
        {
            Debug.Log("SPMonitor Proc Start");

            if (startCountdown)
            {
                foreach (AbyssMonitorController monitor in this.Monitors)
                {
                    monitor.Start();
                }
            }
            else
            {
                foreach (AbyssMonitorController monitor in this.Monitors)
                {
                    monitor.Stop();
                }
            }

            ProcessEnded();
        }

        protected override void ProcessEnded()
        {
            Debug.Log("SPMonitor Proc Ended");
        }

        private void OnCountDownExpired(object sender, EventArgs e)
        {
            if (CountdownExpired != null)
            {
                CountdownExpired(sender, e);
            }
        }

        private void OnCountDownStarted(object sender, EventArgs e)
        {
            if (CountdownStarted != null)
            {
                CountdownStarted(sender, e);
            }
        }

        private void OnCountDownStopped(object sender, EventArgs e)
        {
            if (CountdownStopped != null)
            {
                CountdownStopped(sender, e);
            }
        }

        public SPMonitor()
        {
            this.Name = "SPMonitor";
        }

        public override void Initialize()
        {
            foreach (AbyssMonitorController monitor in this.Monitors)
            {
                monitor.CountdownExpired += this.OnCountDownExpired;
                monitor.CountdownStarted += this.OnCountDownStarted;
                monitor.CountdownStopped += this.OnCountDownStopped;
            }
        }
    }
}
