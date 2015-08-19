package com.locurio.abysstexting;

import android.widget.TextView;

import org.apache.commons.lang3.time.StopWatch;

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
    DateFormat dateFormatter;

    public MessageTimer(MainActivity mainActivity)
    {
        this.timer = new Timer();
        this.schedule = new TimerSchedule(this);
        this.mainActivity = mainActivity;
        this.stopWatch = new StopWatch();
        this.dateFormatter = new SimpleDateFormat("mm:ss");

        timer.scheduleAtFixedRate(schedule, 0, 200);
    }

    public void setTime(long milliseconds)
    {
        resetTime = milliseconds + stopWatch.getTime();
    }

    public void start()
    {
        if(stopWatch.isStopped())
        {
            stopWatch.start();
        }
    }

    public void suspend()
    {
        if(stopWatch.isStarted())
        {
            stopWatch.suspend();
        }
    }

    public void resume()
    {
        if(stopWatch.isSuspended())
        {
            stopWatch.resume();
        }
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
        Debug.log("********milliseconds : " + milliseconds);
        //long minutes = milliseconds / (60 * 1000);
        //long seconds = ceiling(milliseconds, 1000);

        Date date = new Date(milliseconds);
        String prettyTime = dateFormatter.format(date);

        Calendar calendar = Calendar.getInstance();
        calendar.setTime(date);
        int hours = calendar.get(Calendar.HOUR);
        int minutes = calendar.get(Calendar.MINUTE);
        int seconds = calendar.get(Calendar.SECOND);

        prettyTime = String.format("%02d:%02d", (hours * 60) + minutes, seconds);
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
            Debug.log("********Run!");
            messageTimer.updateTimerText();
        }
    }
}
