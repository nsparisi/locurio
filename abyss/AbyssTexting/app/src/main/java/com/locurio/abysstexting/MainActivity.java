package com.locurio.abysstexting;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.net.wifi.WifiManager;
import android.os.Bundle;
import android.os.PowerManager;
import android.app.ActionBar;
import android.widget.ListView;
import android.widget.TextView;
import android.view.View;
import android.app.admin.DevicePolicyManager;
import android.app.admin.DeviceAdminReceiver;
import android.content.ComponentName;

public class MainActivity extends Activity {
    public static class DevAdminReceiver extends DeviceAdminReceiver {
    }

    private ListView messagesList;
    private MessageAdapter messageAdapter;
    MessageListener messageListener = new MessageListener();

    private TextView timerText;

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

        // Similar to wakelock. Aquire a Wifi Lock to prevent Wifi from disconnecting.
        WifiManager wifiManager = (WifiManager) getSystemService(Context.WIFI_SERVICE);
        WifiManager.WifiLock wifiLock = wifiManager.createWifiLock(WifiManager.WIFI_MODE_FULL, "WifiLockTag");
        wifiLock.acquire();

        // setup a timer which will be used to update the app's clock
        timerText = (TextView) findViewById(R.id.TimerText);

        getWindow().getDecorView().setSystemUiVisibility(
                View.SYSTEM_UI_FLAG_IMMERSIVE | View.SYSTEM_UI_FLAG_IMMERSIVE_STICKY | View.SYSTEM_UI_FLAG_FULLSCREEN | View.SYSTEM_UI_FLAG_HIDE_NAVIGATION | View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION | View.SYSTEM_UI_FLAG_LAYOUT_STABLE);

        DevicePolicyManager myDevicePolicyManager = (DevicePolicyManager) getSystemService(Context.DEVICE_POLICY_SERVICE);
        ComponentName mDeviceAdminSample = new ComponentName(this, DevAdminReceiver.class);

        if (myDevicePolicyManager.isDeviceOwnerApp(this.getPackageName())) {
            // Device owner
            String[] packages = {this.getPackageName()};
            myDevicePolicyManager.setLockTaskPackages(mDeviceAdminSample, packages);
        } else {
            // Not a device owner - prompt user or show error
        }

        if (myDevicePolicyManager.isLockTaskPermitted(this.getPackageName())) {
            // Lock allowed
            startLockTask();
        } else {
            // Lock not allowed - show error or something useful here
        }
    }

    // Updates the timer text field.
    public void updateTimer(final String text)
    {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                timerText.setText(text);
            }
        });
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
            MessageService.instance.registerMessageListener(messageListener, this);
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
