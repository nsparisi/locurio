package com.locurio.abysstexting;

import android.app.Activity;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.IBinder;
import android.os.PowerManager;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.ListView;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.InetAddress;
import java.net.ServerSocket;
import java.net.Socket;


public class MainActivity extends Activity {

    private ListView messagesList;
    private MessageAdapter messageAdapter;
    MessageListener messageListener = new MessageListener();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Debug.log("********App started");

        // populate UI components
        messagesList = (ListView) findViewById(R.id.listMessages);
        messageAdapter = new MessageAdapter(this);
        messagesList.setAdapter(messageAdapter);

        // create MessageService which runs in background
        startService(new Intent(this, MessageService.class));

        // wait for MessageService
        Thread waitForService = new Thread(new WaitForService());
        waitForService.start();

        // keep a WakeLock running, which will keep the phone from sleeping
        // I will never release this lock
        PowerManager powerManager = (PowerManager)getSystemService(Context.POWER_SERVICE);
        PowerManager.WakeLock wakeLock = powerManager.newWakeLock(PowerManager.PARTIAL_WAKE_LOCK, "WakeLockTag");
        wakeLock.acquire();
    }

    @Override
    protected void onPause() {
        Debug.log("********onPause");
        super.onPause();
    }

    @Override
    protected void onStop() {
        Debug.log("********onStop");
        super.onStop();
    }

    @Override
    protected void onDestroy() {
        Debug.log("********onDestroy");
        unregisterListener();
        super.onDestroy();
    }

    private void unregisterListener()
    {
        if(MessageService.instance != null) {
            MessageService.instance.unregisterMessageListener(messageListener);
        }
    }

    private void registerListener()
    {
        if(MessageService.instance != null) {
            MessageService.instance.registerMessageListener(messageListener);
        }
    }

    public class MessageListener {
        public void onIncomingMessage(final MessageData message)
        {
            runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    messageAdapter.addMessage(message);
                }
            });
        }

        public void clearMessageHistory()
        {
            runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    messageAdapter.clearMessages();
                }
            });
        }
    }

    public class WaitForService implements Runnable
    {
        @Override
        public void run() {
            while(MessageService.instance == null)
            {
                try {
                    Thread.sleep(200);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }

            registerListener();
        }
    }
}
