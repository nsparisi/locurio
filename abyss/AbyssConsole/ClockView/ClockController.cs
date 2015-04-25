using AbyssLibrary;
using AbyssScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssConsole
{
    public class ClockController
    {
        private List<ClockWindow> allClockWindows;
        private CountDownTimer countdownClock;

        private const long OneHourMs = 60 * 60 * 1000;

        public ClockController(AbyssScreenController screenController)
        {
            allClockWindows = new List<ClockWindow>();

            countdownClock = screenController.CountDownTimer;
            countdownClock.CountDownTicked += OnCountDownTicked;
            countdownClock.SetTime(OneHourMs);
        }

        public void SpawnClock()
        {
            ClockWindow clock = new ClockWindow();
            clock.SetTimeText("12:00:00");
            clock.Show();

            allClockWindows.Add(clock);
        }

        public void CloseAllClockWindows()
        {
            countdownClock.CountDownTicked -= OnCountDownTicked;
            foreach(ClockWindow clock in allClockWindows)
            {
                clock.Close();
            }
        }

        public void StartCountDown()
        {
            countdownClock.Reset();
            countdownClock.SetTime(OneHourMs);
            countdownClock.Start();
        }

        public void StopCountDown()
        {
            countdownClock.Stop();
        }

        public void OnCountDownTicked(object sender, CountDownEventArgs args)
        {
            foreach (ClockWindow clock in allClockWindows)
            {
                clock.SetTime(args.TimeMs);
            }
        }
    }
}
