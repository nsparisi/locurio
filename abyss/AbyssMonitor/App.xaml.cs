using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AbyssMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private List<ClockWindow> allClockWindows;
        CountDownTimer globalCountdown;

        public App() : base()
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

        }
    }
}
