package com.example.myapplication.Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.example.myapplication.R;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class BusStopScheduleAdapter extends ArrayAdapter<Map.Entry<String, List<String>>> {

    private Context mContext;
    private int mResource;


    public BusStopScheduleAdapter(@NonNull Context context, int resource, @NonNull HashMap<String, List<String>> hashMap) {
        super(context, resource, new ArrayList<>(hashMap.entrySet()));
        this.mContext = context;
        this.mResource = resource;
    }

    @NonNull
    @Override
    public View getView(int position, @Nullable View convertView, @NonNull ViewGroup parent) {
        ViewHolder holder;
        if(convertView == null){
            convertView = LayoutInflater.from(getContext()).inflate(R.layout.hour_list, parent, false);
            holder = new ViewHolder();
            holder.keyTextView = convertView.findViewById(R.id.hourBusStop);
            holder.minutesListView = convertView.findViewById(R.id.listMinute);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }

        Map.Entry<String, List<String>> entry = getItem(position);

        if (entry != null) {
            holder.keyTextView.setText(entry.getKey());
            MinuteAdapter minuteAdapter = new MinuteAdapter(mContext, entry.getValue());
            holder.minutesListView.setAdapter(minuteAdapter);
        }

        return convertView;
    }

    static class ViewHolder {
        TextView keyTextView;
        ListView minutesListView;
    }
}
