using AbyssScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class AbyssScreenController : AbstractPhysicalObject
    {
        public event AbyssEvent CountdownExpired;
        public event AbyssEvent CountdownStarted;
        public event AbyssEvent CountdownTicked;

        public CountDownTimer CountDownTimer { get; private set; }

        public AbyssScreenController(string name)
            : base(name)
        {
            CountDownTimer = new CountDownTimer();
            CountDownTimer.CountDownExpired += OnCountDownExpired;
            CountDownTimer.CountDownStarted += OnCountDownStarted;
            CountDownTimer.CountDownTicked += OnCountDownTicked;
        }

        public void Start()
        {
            CountDownTimer.Start();
        }

        public void Stop()
        {
            CountDownTimer.Stop();
        }

        public void SetTime(long milliseconds)
        {
            CountDownTimer.SetTime(milliseconds);
        }

        public void Reset()
        {
            CountDownTimer.Reset();
        }

        private void OnCountDownExpired(object sender, EventArgs e)
        {
            if (CountdownExpired != null)
            {
                CountdownExpired(this, EventArgs.Empty);
            }
        }

        private void OnCountDownStarted(object sender, EventArgs e)
        {
            if (CountdownStarted != null)
            {
                CountdownStarted(this, EventArgs.Empty);
            }
        }

        private void OnCountDownTicked(object sender, EventArgs e)
        {
            if (CountdownTicked != null)
            {
                CountdownTicked(this, EventArgs.Empty);
            }
        }

        // --------
        // Old UI code. Will resue if when the time is right

        Thread screenThread;
        AbyssScreen.App app;
        AbyssScreen.AppController appController;

        private void InitializeWithUI()
        {
            appController = new AbyssScreen.AppController();
            appController.GlobalCountdown.CountDownExpired += this.OnCountDownExpired;
            appController.GlobalCountdown.CountDownStarted += this.OnCountDownStarted;
            appController.GlobalCountdown.CountDownTicked += this.OnCountDownTicked;

            screenThread = new Thread(ScreenThreadStart);
            screenThread.SetApartmentState(ApartmentState.STA);
            screenThread.Start();
        }

        private void ScreenThreadStart()
        {
            app = new AbyssScreen.App(appController);
            app.Run(new AbyssScreen.MainWindow());
        }
    }
}
