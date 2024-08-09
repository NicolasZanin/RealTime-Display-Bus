package com.example.myapplication.Activity;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.View;
import android.widget.ListView;
import android.widget.TextView;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.myapplication.Adapter.BusStopAdapter;
import com.example.myapplication.Adapter.LineListAdapter;
import com.example.myapplication.Item.BusStopItem;
import com.example.myapplication.Item.LineList;
import com.example.myapplication.R;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;


public class SchedulesActivity extends AppCompatActivity {


    private ArrayList<LineList> lineLists = new ArrayList<>();
    private ArrayList<LineList> lineListsFavorite = new ArrayList<>();
    private ArrayList<BusStopItem> busStopItemList = new ArrayList<>();
    LineListAdapter lineListAdapter ;
    BusStopAdapter busStopAdapter;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.schedules_layout);
        createLists();
        changeTitlecolor("line");
        lineListAdapter = new LineListAdapter(SchedulesActivity.this,lineLists);
        ListView listView = findViewById(R.id.listSchedules);
        listView.setAdapter(lineListAdapter);


        findViewById(R.id.homeButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(SchedulesActivity.this, MainActivity.class);
                startActivity(intent);

            }
        });
        findViewById(R.id.trafficInfoButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(SchedulesActivity.this, TrafficInfoActivity.class);
                startActivity(intent);

            }
        });
        findViewById(R.id.titleFavoriteTxt).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                changeTitlecolor("favorite");
                lineListsFavorite = new ArrayList<>();
                for(LineList l : lineLists){
                    if(l.isFavorite()){
                        lineListsFavorite.add(l);
                    }
                }
                lineListAdapter = new LineListAdapter(SchedulesActivity.this,lineListsFavorite);
                listView.setAdapter(lineListAdapter);

            }
        });
        findViewById(R.id.titleLineTxt).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                changeTitlecolor("line");
                lineListAdapter = new LineListAdapter(SchedulesActivity.this,lineLists);
                listView.setAdapter(lineListAdapter);

            }
        });
        findViewById(R.id.titleBusstopTxt).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                changeTitlecolor("busstop");
                busStopAdapter = new BusStopAdapter(SchedulesActivity.this,busStopItemList);
                listView.setAdapter(busStopAdapter);

            }
        });
        listView.setClickable(true);
    }

    private void changeTitlecolor(String title) {
        if(title =="line"){
            TextView textTitle = findViewById(R.id.titleLineTxt);
            TextView text1 = findViewById(R.id.titleFavoriteTxt);
            TextView text2 = findViewById(R.id.titleBusstopTxt);
                text1.setTextColor(Color.parseColor("#FFFFFF"));
            text2.setTextColor(Color.parseColor("#FFFFFF"));
            textTitle.setTextColor(Color.parseColor("#26474E"));

        }
        else if(title =="favorite"){
            TextView textTitle = findViewById(R.id.titleFavoriteTxt);
            TextView text1 = findViewById(R.id.titleLineTxt);
            TextView text2 = findViewById(R.id.titleBusstopTxt);
            text1.setTextColor(Color.parseColor("#FFFFFF"));
            text2.setTextColor(Color.parseColor("#FFFFFF"));
            textTitle.setTextColor(Color.parseColor("#26474E"));

        }
        else{
            TextView textTitle = findViewById(R.id.titleBusstopTxt);
            TextView text1 = findViewById(R.id.titleFavoriteTxt);
            TextView text2 = findViewById(R.id.titleLineTxt);
            text1.setTextColor(Color.parseColor("#FFFFFF"));
            text2.setTextColor(Color.parseColor("#FFFFFF"));
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

        List<String> listArrivee = new ArrayList<>();
        listArrivee.add("Số 34 Nguyễn Lương Bằng");
        listArrivee.add("Kế trụ 504");
        listArrivee.add("Name Station 5");
        listArrivee.add("Số 128 Tôn Đức Thắng");
        listArrivee.add("Kế trụ 504");
        listArrivee.add("Name Station 9");

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
            lineLists.add(new LineList(listImage.get(i),""+(i+1),listDepart.get(i),listArrivee.get(i)));
            busStopItemList.add(new BusStopItem(listDepart.get(i),"Danang,Vietnam",hashMap,hashMap2));
        }


    }


}
