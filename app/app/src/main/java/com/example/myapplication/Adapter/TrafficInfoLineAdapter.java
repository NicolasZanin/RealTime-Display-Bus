package com.example.myapplication.Adapter;

import android.content.Context;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.myapplication.Item.BusStopItem;
import com.example.myapplication.Item.TrafficInfoItem;
import com.example.myapplication.R;

import java.util.ArrayList;
import java.util.List;

public class TrafficInfoLineAdapter extends ArrayAdapter<TrafficInfoItem> {
    private Context mContext;
    public TrafficInfoLineAdapter(@NonNull Context context, ArrayList<TrafficInfoItem> dataArrayList) {
        super(context, R.layout.bus_stop_list,dataArrayList);

        mContext = context;
    }
    @NonNull
    @Override
    public View getView(int position, @Nullable View view, @NonNull ViewGroup parent) {
        TrafficInfoItem trafficInfoItem = getItem(position);

        if(view == null){
            view = LayoutInflater.from(getContext()).inflate(R.layout.listview_alert,parent,false);
        }

        TextView alertTitle = view.findViewById(R.id.alert_title);
        TextView description = view.findViewById(R.id.descriptionAlert);
        ImageView plusBtn = view.findViewById(R.id.moreInfo);
        RecyclerView innerRecyclerView = view.findViewById(R.id.affectedLinesList);

        alertTitle.setText(trafficInfoItem.getTitle());
        description.setText(trafficInfoItem.getDescription());
        InnerTIAdapter innerAdapter = new InnerTIAdapter(trafficInfoItem.getNumbers());
        innerRecyclerView.setAdapter(innerAdapter);
        plusBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if(description.getTextSize()== 0){
                    description.setTextSize(TypedValue.COMPLEX_UNIT_DIP, 24);
                    plusBtn.setRotation(90);
                    ViewGroup.LayoutParams layoutParams = plusBtn.getLayoutParams();
                    layoutParams.width = (int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, 48, v.getResources().getDisplayMetrics());
                    layoutParams.height = (int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, 48, v.getResources().getDisplayMetrics());
                    plusBtn.setLayoutParams(layoutParams);
                }
                else{
                    description.setTextSize(TypedValue.COMPLEX_UNIT_DIP, 0);
                    plusBtn.setRotation(0);
                }
            }
        });
        // Définir le LinearLayoutManager avec une orientation horizontale pour la RecyclerView interne
        innerRecyclerView.setLayoutManager(new LinearLayoutManager(view.getContext(), LinearLayoutManager.HORIZONTAL, false));

        return view;
    }
}
