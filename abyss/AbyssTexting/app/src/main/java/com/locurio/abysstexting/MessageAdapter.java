package com.locurio.abysstexting;

import android.app.Activity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

public class MessageAdapter extends BaseAdapter{
    List<MessageData> messages;
    LayoutInflater layoutInflater;

    public MessageAdapter(Activity activity)
    {
        messages = new ArrayList<MessageData>();
        layoutInflater = activity.getLayoutInflater();
    }

    public void addMessage(MessageData message)
    {
        messages.add(message);
        notifyDataSetChanged();
    }

    @Override
    public int getCount() {
        return messages.size();
    }

    @Override
    public Object getItem(int position) {
        return messages.get(position);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        // todo what is this?
        if(convertView == null)
        {
            int res = R.layout.message;
            convertView = layoutInflater.inflate(res, parent, false);
        }

        // fill in the message view with data
        MessageData messageData = messages.get(position);

        TextView textMessage = (TextView) convertView.findViewById(R.id.txtMessage);
        textMessage.setText(messageData.message);

        TextView textTime = (TextView) convertView.findViewById(R.id.txtTime);
        textTime.setText(messageData.time);
        return convertView;
    }
}
