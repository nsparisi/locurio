using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace AbyssScreen
{
    public class CountDownTimer
    {
        public delegate void CountDownEvent(object sender, CountDownEventArgs args);
        public event CountDownEvent CountDownTicked;
        public event CountDownEvent CountDownStarted;
        public event CountDownEvent CountDownExpired;

        Timer timer;
        long resetTime;
        Stopwatch stopwatch;

        public long Interval { get; set; }
        private long lastCrossedInterval = 0;

        public CountDownTimer()
        {
            timer = new Timer(100);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;

            stopwatch = new Stopwatch();

            Interval = 10;
        }

        public void SetTime(long milliseconds)
        {
            resetTime = milliseconds + stopwatch.ElapsedMilliseconds;
            UpdateListeners();
        }

        public void Start()
        {
            timer.Start();
            stopwatch.Start();
            OnCountDownStarted();
            UpdateListeners();
        }

        public void Stop()
        {
            timer.Stop();
            stopwatch.Stop();
        }

        public void Reset()
        {
            stopwatch.Reset();
            UpdateListeners();
            lastCrossedInterval = 0;
        }

        public long GetTimeRemaining()
        {
            return Math.Max(0, resetTime - stopwatch.ElapsedMilliseconds);
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            long floor = stopwatch.ElapsedMilliseconds / Interval;
            if (lastCrossedInterval < floor)
            {
                lastCrossedInterval = floor;
                UpdateListeners();
            }
        }

        private void UpdateListeners()
        {
            long timeLeft = GetTimeRemaining();

            // time's up
            timeLeft = Math.Max(0, timeLeft);
            if (timeLeft == 0)
            {
                Stop();
                OnCountDownExpired();
            }

            OnCountDownTicked(timeLeft);
        }

        private void OnCountDownExpired()
        {
            if (CountDownExpired != null)
            {
                CountDownExpired(this, new CountDownEventArgs(0));
            }
        }

        private void OnCountDownStarted()
        {
            if (CountDownStarted != null)
            {
                CountDownStarted(this, new CountDownEventArgs(0));
            }
        }

        private void OnCountDownTicked(long timeLeft)
        {
            if (CountDownTicked != null)
            {
                CountDownTicked(this, new CountDownEventArgs(timeLeft));
            }
        }
    }
}
