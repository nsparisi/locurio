package com.locurio.abysstexting;

import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.media.AudioManager;
import android.media.RingtoneManager;
import android.net.Uri;
import android.os.IBinder;
import android.os.PowerManager;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import android.media.Ringtone;

/**
 * Created by Penny on 5/18/2015.
 */
public class MessageService extends Service
{
    public static MessageService instance;

    public static final int SERVER_PORT = 6000;
    private static final String HEARTBEAT = "HEARTBEAT";
    private static final String CLEAR_MESSAGE_HISTORY = "CLEAR_MESSAGE_HISTORY";
    private static final String PING_MESSAGE = "PING_MESSAGE";
    private static final String TIMER_START = "TIMER_START";
    private static final String TIMER_SUSPEND = "TIMER_SUSPEND";
    private static final String TIMER_RESET = "TIMER_RESET";
    private static final String TIMER_SET_TIME = "TIMER_SET_TIME";

    MainActivity.MessageListener messageListener;
    ArrayList<MessageData> allMessages;
    MessageTimer messageTimer;

    ServerSocket serverSocket;
    Thread serverThread;

    @Override
    public void onDestroy()
    {
        Debug.log("********MessageService onDestroy");
        super.onDestroy();
    }

    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    @Override
    public void onCreate()
    {
        Debug.log("********MessageService onCreate");
        if(instance != null)
        {
            Debug.log("********MessageService WARNING instance already exists");
        }

        instance = this;

        allMessages = new ArrayList<MessageData>();
        serverThread = new Thread(new ServerThread());
        serverThread.start();

        Debug.log("********MessageService creating MessageTimer");
        messageTimer = new MessageTimer();
        messageTimer.setTime(70 * 1000 * 60);

        super.onCreate();
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        return super.onStartCommand(intent, flags, startId);
    }

    public void registerMessageListener(MainActivity.MessageListener listener, MainActivity activity)
    {
        Debug.log("********MessageService registerMessageListener");
        if(messageListener != null)
        {
            Debug.log("********MessageService WARNING messageListener already exists");
        }

        messageListener = listener;

        // populate the list with any existing messages
        for(int i = 0; i < allMessages.size(); i++)
        {
            messageListener.onIncomingMessage(allMessages.get(i));
        }

        // set the main activity, which can render the timer text field.
        Debug.log("********MessageService setting activity on timer");
        messageTimer.setActivity(activity);
    }

    public void unregisterMessageListener(MainActivity.MessageListener listener)
    {
        Debug.log("********MessageService unregisterMessageListener");
        messageListener = null;
    }

    private void clearMessageHistory()
    {
        allMessages.clear();
        if(messageListener != null)
        {
            messageListener.clearMessageHistory();
        }
    }

    public class ServerThread implements Runnable
    {
        @Override
        public void run() {
            Socket socket = null;
            try
            {
                Debug.log("********ServerThread started");
                serverSocket = new ServerSocket(SERVER_PORT);
            }
            catch (IOException e)
            {
                e.printStackTrace();
                return;
            }

            while(!Thread.currentThread().isInterrupted())
            {
                try
                {
                    // wait forever until a connection is requested
                    Debug.log("********ServerThread accept");
                    socket = serverSocket.accept();

                    // make a new thread to receive the data
                    Thread t = new Thread(new CommunicationThread(socket));
                    t.run();
                }
                catch (IOException e)
                {
                    e.printStackTrace();
                }
            }
        }
    }

    public class CommunicationThread implements Runnable
    {
        Socket socket;
        public CommunicationThread(Socket socket)
        {
            this.socket = socket;
        }

        @Override
        public void run()
        {
            try
            {
                // this thread will only receive one line of data, then end
                Debug.log("********CommunicationThread getInputStream");
                BufferedReader input = input = new BufferedReader(
                        new InputStreamReader(socket.getInputStream()));

                String line = input.readLine();

                if(line == null || line.isEmpty())
                {
                    Debug.log("********received empty or null message. Exiting");
                }
                else if(line.equals(HEARTBEAT))
                {
                    Debug.log("********received heartbeat request. Exiting");
                }
                else if(line.equals(CLEAR_MESSAGE_HISTORY))
                {
                    Debug.log("********received CLEAR_MESSAGE_HISTORY");
                    clearMessageHistory();
                }
                else if(line.equals(PING_MESSAGE))
                {
                    Debug.log("********received PING_MESSAGE");
                    WakeUpAndPlaySound();
                }
                else if(line.equals(TIMER_START))
                {
                    Debug.log("********TIMER_START");
                    if(messageTimer != null) {
                        messageTimer.start();
                    }
                }
                else if(line.equals(TIMER_SUSPEND))
                {
                    Debug.log("********TIMER_SUSPEND");
                    if(messageTimer != null) {
                        messageTimer.suspend();
                    }
                }
                else if(line.equals(TIMER_RESET))
                {
                    Debug.log("********TIMER_RESET");
                    if(messageTimer != null) {
                        messageTimer.reset();
                    }
                }
                else if(line.startsWith(TIMER_SET_TIME))
                {
                    Debug.log("********TIMER_SET_TIME");
                    String time = line.substring(TIMER_SET_TIME.length());
                    Debug.log("********time " + time);
                    long milliseconds = Long.parseLong(time);
                    Debug.log("********milliseconds " + milliseconds);

                    if(messageTimer != null) {
                        messageTimer.setTime(milliseconds);
                    }
                }
                else
                {
                    // Print message and play a sound
                    MessageData data = new MessageData();
                    data.message = line;
                    data.time = "";

                    allMessages.add(data);
                    if(messageListener != null)
                    {
                        messageListener.onIncomingMessage(data);
                    }

                    WakeUpAndPlaySound();
                }

                Debug.log("********CommunicationThread closing");
                input.close();
                socket.close();
            }
            catch (IOException e)
            {
                e.printStackTrace();
            }
        }

        private void WakeUpAndPlaySound()
        {
            // wake up screen
            Debug.log("********Waking up screen");
            PowerManager powerManager = (PowerManager)getSystemService(Context.POWER_SERVICE);
            PowerManager.WakeLock wakeLock = powerManager.newWakeLock(PowerManager.FULL_WAKE_LOCK | PowerManager.ACQUIRE_CAUSES_WAKEUP | PowerManager.ON_AFTER_RELEASE, "WakeUpScreenTag");
            wakeLock.acquire();
            wakeLock.release();

            // play a sound
            try
            {
                // waiting a second is necessary so the sound doesn't cut off.
                // this is a timing issue when waking up.
                Thread.sleep(1000);

                // set the volume to max
                AudioManager manager = (AudioManager)getSystemService(Context.AUDIO_SERVICE);
                manager.setMode(AudioManager.STREAM_RING);
                manager.setSpeakerphoneOn(true);

                manager.setStreamVolume(
                        AudioManager.STREAM_NOTIFICATION,
                        manager.getStreamMaxVolume(AudioManager.STREAM_NOTIFICATION),
                        0);

                manager.setStreamVolume(
                        AudioManager.STREAM_SYSTEM,
                        manager.getStreamMaxVolume(AudioManager.STREAM_SYSTEM),
                        0);

                manager.setStreamVolume(
                        AudioManager.STREAM_RING,
                        manager.getStreamMaxVolume(AudioManager.STREAM_RING),
                        0);


                manager.setStreamVolume(
                        AudioManager.STREAM_MUSIC,
                        manager.getStreamMaxVolume(AudioManager.STREAM_MUSIC),
                        0);

                manager.setMode(AudioManager.MODE_RINGTONE);

                Thread.sleep(500);

                // play the notification
                Uri notificationUri = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);
                Ringtone ringtone = RingtoneManager.getRingtone(getApplicationContext(), notificationUri);
                ringtone.setStreamType(AudioManager.STREAM_MUSIC);
                //ringtone.setVolume(1.0f);
                ringtone.play();
            }
            catch (Exception e)
            {
                e.printStackTrace();
            }
        }
    }
}
