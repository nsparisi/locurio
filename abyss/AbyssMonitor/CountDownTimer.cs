using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace AbyssMonitor
{
    public class CountDownTimer
    {
        public delegate void CountDownEvent(long time);
        public event CountDownEvent OnTimeChanged;

        public delegate void CountDownExpiredEvent();
        public event CountDownExpiredEvent OnCountDownExpired;

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
            resetTime = milliseconds;
            UpdateListeners();
        }

        public void Start()
        {
            timer.Start();
            stopwatch.Start();
            UpdateListeners();
        }

        public void Stop()
        {
            timer.Stop();
            stopwatch.Stop();
            UpdateListeners();
        }

        public void Reset()
        {
            stopwatch.Reset();
            UpdateListeners();
            lastCrossedInterval = 0;
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
            long timeLeft = resetTime - stopwatch.ElapsedMilliseconds;

            // time's up
            timeLeft = Math.Max(0, timeLeft);
            if (timeLeft == 0)
            {
                CountDownExpired();
            }

            if(OnTimeChanged != null)
            {
                OnTimeChanged(timeLeft);
            }
        }

        private void CountDownExpired()
        {
            if (OnCountDownExpired != null)
            {
                OnCountDownExpired();
            }
        }
    }
}
