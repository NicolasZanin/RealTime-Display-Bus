package com.example.myapplication.Activity;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.myapplication.Adapter.BusStopAdapter;
import com.example.myapplication.Adapter.TrafficInfoAdapter;
import com.example.myapplication.Adapter.TrafficInfoLineAdapter;
import com.example.myapplication.Item.BusStopItem;
import com.example.myapplication.Item.LineList;
import com.example.myapplication.Item.TrafficInfoItem;
import com.example.myapplication.R;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;

public class ScheduleLineActivity extends AppCompatActivity {
    private BusStopAdapter busStopAdapter;
    private ArrayList<TrafficInfoItem> outerList = new ArrayList<>();
    private TrafficInfoLineAdapter trafficInfoLineAdapter;
    private ArrayList<BusStopItem> busStopItemList = new ArrayList<>();
    Boolean sens = true;
    private String nbLine;
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.schedule_line_activity);
        LineList lineList = getIntent().getParcelableExtra("lineList");
        TextView t1 = findViewById(R.id.busStopFirst);
        t1.setText("from: "+lineList.getDepart());
        TextView t2 = findViewById(R.id.busStopSecond);
        t2.setText("to: "+lineList.getArrivee());
        ImageView imageView = findViewById(R.id.nbLine);
        imageView.setImageResource(lineList.getImage());
        nbLine = lineList.getNbLine();
        changeTitlecolor("busStop");
        ListView listView = findViewById(R.id.listSchedulesLine);
        createLists();
        createAlert();
        busStopAdapter = new BusStopAdapter(this,busStopItemList);
        listView.setAdapter(busStopAdapter);
        findViewById(R.id.homeButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(ScheduleLineActivity.this, MainActivity.class);
                startActivity(intent);

            }
        });
        findViewById(R.id.trafficInfoButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(ScheduleLineActivity.this, TrafficInfoActivity.class);
                startActivity(intent);

            }
        });
        findViewById(R.id.retourScheduleLine).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(ScheduleLineActivity.this, SchedulesActivity.class);
                startActivity(intent);

            }
        });
        findViewById(R.id.switchStationBTN).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                TextView t1 = findViewById(R.id.busStopFirst);
                String busStop1 = t1.getText().toString().split(":")[1];
                TextView t2 = findViewById(R.id.busStopSecond);
                String busStop2 = t2.getText().toString().split(":")[1];
                t1.setText("from:"+busStop1);
                t2.setText("to:"+busStop2);
                sens = !sens;

            }
        });
        findViewById(R.id.busStopBTN).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                changeTitlecolor("busStop");
                listView.setAdapter(busStopAdapter);
            }
        });

        findViewById(R.id.trafficInfoBTN).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                changeTitlecolor("trafficInfo");
                ArrayList<TrafficInfoItem> tmp = new ArrayList<>();
                for (TrafficInfoItem t : outerList){

                    if(t.getNumbers().contains(nbLine)){
                        tmp.add(t);
                    }
                }

                trafficInfoLineAdapter = new TrafficInfoLineAdapter(ScheduleLineActivity.this,tmp);
                listView.setAdapter(trafficInfoLineAdapter);

            }
        });


    }
    private void changeTitlecolor(String title) {
        if(title =="busStop"){
            TextView textTitle = findViewById(R.id.busStopBTN);
            TextView text1 = findViewById(R.id.trafficInfoBTN);
            text1.setTextColor(Color.parseColor("#FFFFFF"));
            textTitle.setTextColor(Color.parseColor("#26474E"));

        }
        else{
            TextView textTitle = findViewById(R.id.trafficInfoBTN);
            TextView text1 = findViewById(R.id.busStopBTN);
            text1.setTextColor(Color.parseColor("#FFFFFF"));
            textTitle.setTextColor(Color.parseColor("#26474E"));

        }
    }
    private void createLists(){
        List<Integer> listImage = new ArrayList<>();
        listImage.add(R.drawable.line1);
        listImage.add(R.drawable.line2);
        listImage.add(R.drawable.line3);
        listImage.add(R.drawable.line4);
        listImage.add(R.drawable.line5);
        listImage.add(R.drawable.line6);

        List<String> listDepart = new ArrayList<>();
        listDepart.add("Số 34 Nguyễn Lương Bằng");
        listDepart.add("Kế trụ 504");
        listDepart.add("Name Station 5");
        listDepart.add("Số 128 Tôn Đức Thắng");
        listDepart.add("Kế trụ 504");
        listDepart.add("Name Station 9");


        List<String> horaires = new ArrayList<>();
        horaires.add("5h38");
        horaires.add("5h53");
        horaires.add("6h08");
        horaires.add("6h18");
        horaires.add("6h28");
        horaires.add("6h38");
        horaires.add("6h53");
        horaires.add("7h08");
        horaires.add("7h28");
        horaires.add("7h48");
        horaires.add("8h08");
        horaires.add("8h38");
        horaires.add("9h08");
        horaires.add("9h38");
        horaires.add("10h08");

        HashMap<String,List<String>> hashMap = new HashMap<>();

        hashMap.put("5",horaires);
        List<String> horaires2 = new ArrayList<>();
        horaires2.add("5h33");
        horaires2.add("5h55");
        horaires2.add("6h48");
        horaires2.add("6h58");

        horaires2.add("7h07");

        HashMap<String,List<String>> hashMap2 = new HashMap<>();

        hashMap2.put("5",horaires2);
        for(int i = 0;i<listDepart.size();i++){
            busStopItemList.add(new BusStopItem(listDepart.get(i),"Danang,Vietnam",hashMap,hashMap2));
        }


    }

    public void createAlert(){
        outerList.add(new TrafficInfoItem("Alert Title 1","descrition Alert 1", Arrays.asList("1", "2", "3", "4", "5", "6")));
        outerList.add(new TrafficInfoItem("Alert Title 2","descrition Alert 2", Arrays.asList( "1","5")));
        outerList.add(new TrafficInfoItem("Alert Title 3","descrition Alert 3", Arrays.asList("1", "2", "3", "4", "5", "6")));
        outerList.add(new TrafficInfoItem("Alert Title 4","descrition Alert 4", Arrays.asList("3")));
        outerList.add(new TrafficInfoItem("Alert Title 5","descrition Alert 5", Arrays.asList("1", "5")));

    }
}
