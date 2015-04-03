using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class AbyssScreenController
    {
        public event AbyssEvent CountdownExpired;
        public event AbyssEvent CountdownStopped;
        public event AbyssEvent CountdownStarted;

        Thread screenThread;
        AbyssScreen.App app;
        AbyssScreen.AppController appController;

        public AbyssScreenController()
        {
            appController = new AbyssScreen.AppController();
            appController.CountdownExpired += this.OnCountDownExpired;
            appController.CountdownStarted += this.OnCountDownStarted;
            appController.CountdownStopped += this.OnCountDownStopped;

            screenThread = new Thread(ScreenThreadStart);
            screenThread.SetApartmentState(ApartmentState.STA);
            screenThread.Start();
        }

        private void ScreenThreadStart()
        {
            app = new AbyssScreen.App(appController);
            app.Run(new AbyssScreen.MainWindow());
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
