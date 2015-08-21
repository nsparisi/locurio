package com.locurio.abysstexting;

import android.widget.TextView;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.Timer;
import java.util.TimerTask;

public class MessageTimer {

    MainActivity mainActivity;
    TimerSchedule schedule;
    Timer timer;
    StopWatch stopWatch;

    long resetTime;
    Calendar calendar;

    Date startDate = null;

    public MessageTimer(MainActivity mainActivity)
    {
        this.timer = new Timer();
        this.schedule = new TimerSchedule(this);
        this.mainActivity = mainActivity;
        this.stopWatch = new StopWatch();
        this.calendar = Calendar.getInstance();

        timer.scheduleAtFixedRate(schedule, 0, 200);
    }

    public void setTime(long milliseconds)
    {
        resetTime = milliseconds + stopWatch.getTime();
    }

    public void start()
    {
        stopWatch.start();
    }

    public void suspend()
    {
        stopWatch.suspend();
    }

    public void reset()
    {
        stopWatch.reset();
    }

    public long getTimeRemaining()
    {
        return Math.max(0, resetTime - stopWatch.getTime());
    }

    void updateTimerText()
    {
        long milliseconds = getTimeRemaining();

        Date date = new Date(milliseconds);
        calendar.setTime(date);
        int hours = calendar.get(Calendar.HOUR);
        int minutes = calendar.get(Calendar.MINUTE);
        int seconds = calendar.get(Calendar.SECOND);

        long m = milliseconds / (60 * 1000);
        long s = (milliseconds / 1000) % 60;

        String prettyTime = String.format("%02d:%02d", m, s);
        mainActivity.updateTimer(prettyTime);
    }

    private long ceiling(long dividend, long divisor)
    {
        long quotient = dividend / divisor;
        long remainder = dividend % divisor;

        if(remainder > 0)
        {
            quotient++;
        }

        return quotient;
    }

    /**
     * This is a simple scheduler which calls an update on the timer a couple times a second.
     */
    private class TimerSchedule extends TimerTask
    {
        MessageTimer messageTimer;
        public TimerSchedule(MessageTimer messageTimer)
        {
            this.messageTimer = messageTimer;
        }

        @Override
        public void run() {
            messageTimer.updateTimerText();
        }
    }

    private class StopWatch
    {
        private long start = 0;
        private long suspendedTime = 0;
        private boolean isSuspended = true;

        public void start()
        {
            if(isSuspended) {
                isSuspended = false;
                long oldTime = suspendedTime - start;
                start = System.currentTimeMillis() - oldTime;
            }
        }

        public void suspend()
        {
            if(!isSuspended) {
                isSuspended = true;
                suspendedTime = System.currentTimeMillis();
            }
        }

        public void reset()
        {
            suspend();
            start = System.currentTimeMillis();
        }

        public long getTime() {
            long now = System.currentTimeMillis();
            if(isSuspended)
            {
                now = suspendedTime;
            }
            return now - start;
        }
    }
}
