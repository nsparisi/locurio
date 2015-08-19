package com.locurio.abysstexting;

import android.app.Service;
import android.content.Context;
import android.content.Intent;
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

/**
 * Created by Penny on 5/18/2015.
 */
public class MessageService extends Service
{
    public static MessageService instance;

    public static final int SERVER_PORT = 6000;
    private static final String CLEAR_MESSAGE_HISTORY = "CLEAR_MESSAGE_HISTORY";

    MainActivity.MessageListener messageListener;
    ArrayList<MessageData> allMessages;

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

        super.onCreate();
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        return super.onStartCommand(intent, flags, startId);
    }

    public void registerMessageListener(MainActivity.MessageListener listener)
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

                if(line != null && line.equals(CLEAR_MESSAGE_HISTORY))
                {
                    clearMessageHistory();
                }
                else if(line != null)
                {
                    // todo, split message into parts
                    MessageData data = new MessageData();
                    data.message = line;
                    data.time = "";

                    allMessages.add(data);
                    if(messageListener != null)
                    {
                        messageListener.onIncomingMessage(data);
                    }

                    // play a sound
                    try
                    {
                        Uri notificationUri = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);
                        RingtoneManager.getRingtone(getApplicationContext(), notificationUri).play();
                    }
                    catch (Exception e)
                    {
                        e.printStackTrace();
                    }

                    // wake up screen
                    Debug.log("********Waking up screen");
                    PowerManager powerManager = (PowerManager)getSystemService(Context.POWER_SERVICE);
                    PowerManager.WakeLock wakeLock = powerManager.newWakeLock(PowerManager.FULL_WAKE_LOCK | PowerManager.ACQUIRE_CAUSES_WAKEUP | PowerManager.ON_AFTER_RELEASE, "WakeUpScreenTag");
                    wakeLock.acquire();
                    wakeLock.release();
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
    }
}
