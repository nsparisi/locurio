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
        private CountDownTimer countdownClock;

        private const long OneHourMs = 2 * 60 * 60 * 1000;
        private const long OneMinuteMs = 1 * 60 * 1000;

        public ClockController(AbyssScreenController screenController)
        {

            countdownClock = screenController.CountDownTimer;
            countdownClock.SetTime(OneHourMs);
        }

        public void StartCountDown()
        {
            countdownClock.Reset();
            countdownClock.SetTime(OneHourMs);
            countdownClock.Start();
        }

        public void StartCountDown(int minutes)
        {
            countdownClock.Reset();
            countdownClock.SetTime(minutes * 60 * 1000);
            countdownClock.Start();
        }

        public static string GetPrettyTimeText(long milliseconds)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(milliseconds);
            /*long hours = milliseconds / (60 * 60 * 1000);
            long minutes = milliseconds / (60 * 1000);
            long seconds = Ceiling(milliseconds, (1000));
            string prettyTime = string.Format(
                "{0}:{1}:{2}",
                hours.ToString("00"),
                minutes.ToString("00"),
                seconds.ToString("00"));*/
            string prettyTime = string.Format(
                "{0}:{1}",
                (t.Minutes + t.Hours * 60).ToString("00"),
                t.Seconds.ToString("00"));
            return prettyTime;
        }
    }
}
