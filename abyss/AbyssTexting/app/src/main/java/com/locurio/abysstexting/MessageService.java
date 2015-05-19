package com.locurio.abysstexting;

import android.app.Service;
import android.content.Intent;
import android.os.Binder;
import android.os.IBinder;
import android.os.Trace;
import android.support.v4.content.LocalBroadcastManager;
import android.util.Log;

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
    MessageServiceInterface serviceInterface = new MessageServiceInterface();
    MainActivity.MessageListener messageListener;
    ArrayList<MessageData> allMessages;

    ServerSocket serverSocket;
    Thread serverThread;
    public static final int SERVER_PORT = 6000;

    // todo probably dont need broadcaster
    //LocalBroadcastManager broadcaster;
    //Intent broadcastIntent = new Intent("com.locurio.abysstexting.MainActivity");

    @Override
    public void onDestroy()
    {
        super.onDestroy();
    }

    @Override
    public void onCreate()
    {
        allMessages = new ArrayList<MessageData>();
        serverThread = new Thread(new ServerThread());
        serverThread.start();

        super.onCreate();
    }

    @Override
    public IBinder onBind(Intent intent)
    {
        return serviceInterface;
    }

    public class MessageServiceInterface extends Binder
    {
        public void addMessageListener(MainActivity.MessageListener listener)
        {
            messageListener = listener;

            // populate the list with any existing messages
            for(int i = 0; i < allMessages.size(); i++)
            {
                messageListener.onIncomingMessage(allMessages.get(i));
            }
        }

        public void removeMessageListener(MainActivity.MessageListener messageListener)
        {
            messageListener = null;
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
                BufferedReader input = input = new BufferedReader(
                        new InputStreamReader(socket.getInputStream()));

                String line = input.readLine();

                if(line != null)
                {
                    // todo, split message into parts
                    MessageData data = new MessageData();
                    data.message = line;
                    data.time = "7:21 PM";

                    allMessages.add(data);
                    if(messageListener != null)
                    {
                        messageListener.onIncomingMessage(data);
                    }
                }

                input.close();
                socket.close();
            }
            catch (IOException e)
            {
                e.printStackTrace();
            }
        }
    }

    public class ServerThread implements Runnable
    {
        @Override
        public void run() {
            Socket socket = null;
            try
            {
                Log.v("MainActivity", "********Service started");
                serverSocket = new ServerSocket(SERVER_PORT);
            }
            catch (IOException e)
            {
                e.printStackTrace();
            }

            while(!Thread.currentThread().isInterrupted())
            {
                try
                {
                    // wait forever until a connection is requested
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
}
