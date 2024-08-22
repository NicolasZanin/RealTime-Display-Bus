package com.example.myapplication.Activity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.myapplication.Adapter.BusStopScheduleAdapter;
import com.example.myapplication.Item.BusStopItem;
import com.example.myapplication.Item.LineList;
import com.example.myapplication.R;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.TreeMap;

public class BusStopScheduleActivity extends AppCompatActivity {
    private HashMap<Integer,List<String>> listHoraire = new HashMap<>();
    private BusStopScheduleAdapter busStopScheduleAdapter;
    private Boolean aller;
    private LineList line;
    private BusStopItem busStop;
    private TreeMap<Integer, List<String>> horairesTries;
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.bus_stop_schedule_layout);
         busStop = getIntent().getParcelableExtra("busStop");
        aller = getIntent().getBooleanExtra("aller",true);
        line = getIntent().getParcelableExtra("line");
        TextView busStopNameTextView = findViewById(R.id.busStopFirst);
        ImageView imageView = findViewById(R.id.nbLine);
        switch (line.getNbLine()){
            case "1":
                imageView.setBackgroundResource(R.drawable.line1);
                break;
            case "2":
                imageView.setBackgroundResource(R.drawable.line2);
                break;
            case "3":
                imageView.setBackgroundResource(R.drawable.line3);
                break;
            case "4":
                imageView.setBackgroundResource(R.drawable.line4);
                break;
            case "5":
                imageView.setBackgroundResource(R.drawable.line5);
                break;
            case "6":
                imageView.setBackgroundResource(R.drawable.line6);
                break;
            default:
                imageView.setBackgroundResource(R.drawable.line6);
                break;
        }
        busStopNameTextView.setText(busStop.getName());
        if(aller)
            setHM(busStop.getSchedulesAller());
        else{
            setHM(busStop.getSchedulesRetour());
        }

        busStopScheduleAdapter = new BusStopScheduleAdapter(this,R.layout.hour_list,horairesTries);
        ListView listView = findViewById(R.id.listSchedulesBusStop);
        listView.setAdapter(busStopScheduleAdapter);

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
                if(aller) {
                    TextView textView = findViewById(R.id.busStopSecond);
                    textView.setText("to: " + line.getArrivee());
                    setHM(busStop.getSchedulesAller());
                }
                else{
                    TextView textView = findViewById(R.id.busStopSecond);
                    textView.setText("to: " + line.getDepart());
                    setHM(busStop.getSchedulesRetour());
                }

                busStopScheduleAdapter = new BusStopScheduleAdapter(BusStopScheduleActivity.this,R.layout.hour_list,horairesTries);
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

                if (!listHoraire.containsKey(Integer.parseInt(tab[0]))) {
                    List<String> tmp = new ArrayList<>();
                    tmp.add(tab[1]);
                    listHoraire.put(Integer.parseInt(tab[0]), tmp);
                } else {
                    listHoraire.get(Integer.parseInt(tab[0])).add(tab[1]);
                }

            }
        }
         horairesTries = new TreeMap<>(listHoraire);
        System.out.println(listHoraire);
    }
}
