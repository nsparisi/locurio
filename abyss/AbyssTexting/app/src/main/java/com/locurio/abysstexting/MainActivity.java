package com.locurio.abysstexting;

import android.app.Activity;
import android.content.ComponentName;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.IBinder;
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

    private MessageService.MessageServiceInterface messageService;
    private ListView messagesList;
    private MessageAdapter messageAdapter;
    MessageListener messageListener = new MessageListener();
    MyServiceConnection serviceConnection = new MyServiceConnection();

    private static final String TAG = "MainActivity";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Log.v(TAG, "********App started");

        bindService(new Intent(this, MessageService.class), serviceConnection, BIND_AUTO_CREATE);

        messagesList = (ListView) findViewById(R.id.listMessages);
        messageAdapter = new MessageAdapter(this);
        messagesList.setAdapter(messageAdapter);
        populateMessageHistory();
    }

    private void populateMessageHistory()
    {
        //todo
        //AddMessage("Test 123");
        //AddMessage("Test 333");
        //AddMessage("Test 666");
    }

    private void AddMessage(String message)
    {
        MessageData data = new MessageData();
        data.message = message;
        data.time = "12:44 PM";
        AddMessage(data);
    }

    private void AddMessage(final MessageData message)
    {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                messageAdapter.addMessage(message);
            }
        });
    }

    @Override
    protected void onDestroy()
    {
        messageService.removeMessageListener(messageListener);
        unbindService(serviceConnection);
        super.onDestroy();
    }

    private class MyServiceConnection implements ServiceConnection
    {
        @Override
        public void onServiceConnected(ComponentName name, IBinder service)
        {
            messageService = (MessageService.MessageServiceInterface)service;
            messageService.addMessageListener(messageListener);
        }

        @Override
        public void onServiceDisconnected(ComponentName name)
        {
            messageService = null;
        }
    }

    public class MessageListener {
        public void onIncomingMessage(MessageData message)
        {
            AddMessage(message);
        }
    }
}
