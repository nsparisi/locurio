using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class AbyssMonitorController
    {
        public event AbyssEvent CountdownExpired;
        public event AbyssEvent CountdownStopped;
        public event AbyssEvent CountdownStarted;

        Thread monitorThread;
        AbyssMonitor.App app;
        AbyssMonitor.AppController appController;

        public AbyssMonitorController()
        {
            appController = new AbyssMonitor.AppController();
            appController.CountdownExpired += this.OnCountDownExpired;
            appController.CountdownStarted += this.OnCountDownStarted;
            appController.CountdownStopped += this.OnCountDownStopped;

            monitorThread = new Thread(MonitorThreadStart);
            monitorThread.SetApartmentState(ApartmentState.STA);
            monitorThread.Start();
        }

        private void MonitorThreadStart()
        {
            app = new AbyssMonitor.App(appController);
            app.Run(new AbyssMonitor.MainWindow());
        }

        public void Start()
        {
            Debug.Log("Starting");
            appController.StartCountDown();
        }

        public void Stop()
        {
            Debug.Log("Stopping");
            appController.StopCountDown();
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
    }
}
