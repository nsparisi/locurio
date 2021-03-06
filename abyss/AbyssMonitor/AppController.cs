﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssScreen
{
    public class AppController
    {
        private List<ClockWindow> allClockWindows;
        public CountDownTimer GlobalCountdown { get; private set; }

        public AppController()
        {
            allClockWindows = new List<ClockWindow>();

            GlobalCountdown = new CountDownTimer();
            GlobalCountdown.CountDownTicked += OnUpdateClock;
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
            GlobalCountdown.SetTime(startTime);

            GlobalCountdown.Reset();
            GlobalCountdown.Start();
        }

        public void StopCountDown()
        {
            GlobalCountdown.Stop();
        }

        public void OnUpdateClock(object sender, CountDownEventArgs args)
        {
            foreach (ClockWindow clock in allClockWindows)
            {
                clock.SetTime(args.TimeMs);
            }
        }
    }
}
