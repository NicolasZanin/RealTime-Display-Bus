package com.example.myapplication.Activity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ListView;
import android.widget.TextView;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.myapplication.Adapter.BusStopScheduleAdapter;
import com.example.myapplication.Item.BusStopItem;
import com.example.myapplication.R;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class BusStopScheduleActivity extends AppCompatActivity {
    private HashMap<String,List<String>> listHoraire = new HashMap<>();
    private BusStopScheduleAdapter busStopScheduleAdapter;
    private Boolean aller;
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.bus_stop_schedule_layout);
        BusStopItem busStop = getIntent().getParcelableExtra("busStop");
        aller = getIntent().getBooleanExtra("aller",true);

        TextView busStopNameTextView = findViewById(R.id.busStopFirst);

        busStopNameTextView.setText(busStop.getName());
        if(aller)
            setHM(busStop.getSchedulesAller());
        else{
            setHM(busStop.getSchedulesRetour());
        }
        busStopScheduleAdapter = new BusStopScheduleAdapter(this,R.layout.hour_list,listHoraire);
        ListView listView = findViewById(R.id.listSchedulesBusStop);
        listView.setAdapter(busStopScheduleAdapter);
        for(String s : listHoraire.keySet()){
            System.out.println(listHoraire.get(s).toString());
        }
        findViewById(R.id.retourBusStop).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(BusStopScheduleActivity.this, SchedulesActivity.class);

                startActivity(intent);
            }
        });
        findViewById(R.id.homeButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(BusStopScheduleActivity.this, MainActivity.class);
                startActivity(intent);

            }
        });
        findViewById(R.id.trafficInfoButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(BusStopScheduleActivity.this, TrafficInfoActivity.class);
                startActivity(intent);

            }
        });
        findViewById(R.id.divToBusStop).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                aller = !aller;
                if(aller)
                    setHM(busStop.getSchedulesAller());
                else{
                    setHM(busStop.getSchedulesRetour());
                }

                busStopScheduleAdapter = new BusStopScheduleAdapter(BusStopScheduleActivity.this,R.layout.hour_list,listHoraire);
                ListView listView = findViewById(R.id.listSchedulesBusStop);
                listView.setAdapter(busStopScheduleAdapter);
            }
        });
    }

    private void setHM(HashMap<String,List<String>> schedulesHM) {
        listHoraire = new HashMap<>();
        for(List<String> schedules : schedulesHM.values()) {
            for (String s : schedules) {
                String[] tab = s.split("h");
                if (!listHoraire.containsKey(tab[0])) {
                    List<String> tmp = new ArrayList<>();
                    tmp.add(tab[1]);
                    listHoraire.put(tab[0], tmp);
                } else {
                    listHoraire.get(tab[0]).add(tab[1]);
                }

            }
        }
    }
}
