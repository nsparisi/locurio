using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssScreen
{
    public class AppController
    {
        private List<ClockWindow> allClockWindows;
        CountDownTimer globalCountdown;

        public delegate void AbyssScreenEvent(object sender, EventArgs e);
        public event AbyssScreenEvent CountdownExpired;
        public event AbyssScreenEvent CountdownStarted;
        public event AbyssScreenEvent CountdownStopped;

        public AppController()
        {
            allClockWindows = new List<ClockWindow>();

            globalCountdown = new CountDownTimer();
            globalCountdown.OnTimeChanged += OnUpdateClock;
            globalCountdown.OnCountDownExpired += OnCountdownExpired;
        }

        public void SpawnClock()
        {
            ClockWindow clock = new ClockWindow();
            clock.SetTimeText("12:00:00");
            clock.Show();

            allClockWindows.Add(clock);
        }

        public void ExitProgram()
        {
            foreach(ClockWindow clock in allClockWindows)
            {
                clock.Close();
            }
        }

        public void StartCountDown()
        {
            long startTime = 10000;
            globalCountdown.SetTime(startTime);

            globalCountdown.Reset();
            globalCountdown.Start();

            if (CountdownStarted != null)
            {
                CountdownStarted(this, EventArgs.Empty);
            }
        }

        public void StopCountDown()
        {
            globalCountdown.Stop();

            if (CountdownStopped != null)
            {
                CountdownStopped(this, EventArgs.Empty);
            }
        }

        public void OnUpdateClock(long time)
        {
            foreach (ClockWindow clock in allClockWindows)
            {
                clock.SetTime(time);
            }
        }

        public void OnCountdownExpired()
        {
            if(CountdownExpired != null)
            {
                CountdownExpired(this, EventArgs.Empty);
            }
        }
    }
}
