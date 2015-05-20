package com.locurio.abysstexting;

import android.util.Log;

/**
 * Created by Penny on 5/19/2015.
 */
public class Debug {

    private static final String TAG = "LocurioMessaging";
    public static void log(String message)
    {
        Log.v(TAG, message);
    }
}
