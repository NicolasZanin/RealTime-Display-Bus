package com.example.myapplication.Activity;

import android.content.Context;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Build;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.TextView;

import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;

import com.example.myapplication.Adapter.RoadInfoAdapter;
import com.example.myapplication.Adapter.SearchRoadAdapter;
import com.example.myapplication.Item.BusStopItem;
import com.example.myapplication.Item.DataBase;
import com.example.myapplication.Item.RoadInfoItem;
import com.example.myapplication.R;

import java.time.LocalTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class SearchRoadActivity extends AppCompatActivity {

    private ArrayList<BusStopItem> busStopItems ;
    private SearchRoadAdapter searchRoadAdapter;
    private RoadInfoAdapter roadInfoAdapter;
    private ListView listRoads;
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.search_road_layout);

        busStopItems = new ArrayList<>();
        busStopItems.addAll(DataBase.getInstance().getStations());


        listRoads = findViewById(R.id.listRoads);
        TextView departure = findViewById(R.id.departure);
        TextView arrival = findViewById(R.id.arrival);
        searchRoadAdapter = new SearchRoadAdapter(this,busStopItems,departure,arrival);
        listRoads.setAdapter(searchRoadAdapter);



        findViewById(R.id.schedulesButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(SearchRoadActivity.this, SchedulesActivity.class);
                startActivity(intent);

            }
        });
        findViewById(R.id.trafficInfoButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(SearchRoadActivity.this, TrafficInfoActivity.class);
                startActivity(intent);

            }
        });
        findViewById(R.id.homeButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(SearchRoadActivity.this, MainActivity.class);
                startActivity(intent);

            }
        });
        findViewById(R.id.retourSearchRoad).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(SearchRoadActivity.this, MainActivity.class);
                startActivity(intent);

            }
        });
        EditText editText =  findViewById(R.id.searchStop);
        editText.setOnFocusChangeListener(new View.OnFocusChangeListener() {
            @Override
            public void onFocusChange(View v, boolean hasFocus) {
                if (hasFocus) {
                    listRoads.setVisibility(View.INVISIBLE);
                    listRoads.setClickable(false);
                } else {
                    listRoads.setVisibility(View.VISIBLE);
                    listRoads.setClickable(true);
                }
            }
        });
        editText.setOnKeyListener(new View.OnKeyListener() {
            @Override
            public boolean onKey(View v, int keyCode, KeyEvent event) {
                if (event.getAction() == KeyEvent.ACTION_DOWN && keyCode == KeyEvent.KEYCODE_ENTER) {

                    String stationSearch = editText.getText().toString();
                    ArrayList<BusStopItem> tmp = new ArrayList<>();

                    for(BusStopItem busStopItem : busStopItems){
                        if(busStopItem.getName().contains(stationSearch)){
                            tmp.add(busStopItem);
                        }
                    }
                    listRoads.setVisibility(View.VISIBLE);
                    listRoads.setClickable(true);
                    searchRoadAdapter = new SearchRoadAdapter(SearchRoadActivity.this,tmp,departure,arrival);
                    listRoads.setAdapter(searchRoadAdapter);
                    return true;
                }
                return false;
            }
        });
    }
    @Override
    public boolean dispatchTouchEvent(MotionEvent event) {
        if (event.getAction() == MotionEvent.ACTION_DOWN) {
            View view = getCurrentFocus();
            if (view instanceof EditText) {
                Rect outRect = new Rect();
                view.getGlobalVisibleRect(outRect);
                if (!outRect.contains((int)event.getRawX(), (int)event.getRawY())) {
                    view.clearFocus();
                    InputMethodManager imm = (InputMethodManager) getSystemService(Context.INPUT_METHOD_SERVICE);
                    imm.hideSoftInputFromWindow(view.getWindowToken(), 0);
                }
            }
        }
        return super.dispatchTouchEvent(event);
    }
    @RequiresApi(api = Build.VERSION_CODES.O)
    public void findRoads(BusStopItem b1, BusStopItem b2){
        List<String> aller = new ArrayList<>();
        List<String> retour = new ArrayList<>();
        ArrayList<RoadInfoItem> roadList = new ArrayList<>();
        LocalTime now = LocalTime.now();
        //vérifier si l'arret est b1 est avant ou après b2 sur le trajet Aller
        for(String line : b1.getSchedulesAller().keySet()){
            if(b2.getSchedulesAller().keySet().contains(line)){
              String b1Horaire =  b1.getSchedulesAller().get(line).get(0);
              String b2Horaire =  b2.getSchedulesAller().get(line).get(0);
              if(convertScheduleToInt(b1Horaire)<convertScheduleToInt(b2Horaire)){
                aller.add(line);
              }else{
                  retour.add(line);
              }
            }
        }
        for(String s : aller){
            for(int i = 0;i<b1.getSchedulesAller().get(s).size();i++){
                int minutesSinceMidnight = now.getHour() * 60 + now.getMinute();
                //si la date actuelle est inférieure à l'horaire du bus
                if(minutesSinceMidnight<convertScheduleToInt(b1.getSchedulesAller().get(s).get(i))) {
                    roadList.add(new RoadInfoItem(s,
                            b1.getSchedulesAller().get(s).get(i),
                            b2.getSchedulesAller().get(s).get(i),
                            convertScheduleToInt(b1.getSchedulesAller().get(s).get(i))-minutesSinceMidnight));
                }
            }
        }
        for(String s : retour){
            for(int i = 0;i<b1.getSchedulesAller().get(s).size();i++){
                int minutesSinceMidnight = now.getHour() * 60 + now.getMinute();
                 //si la date actuelle est inférieure à l'horaire du bus
                if(minutesSinceMidnight<convertScheduleToInt(b1.getSchedulesAller().get(s).get(i))) {
                    roadList.add(new RoadInfoItem(s,
                            b1.getSchedulesAller().get(s).get(i),
                            b2.getSchedulesAller().get(s).get(i),
                            convertScheduleToInt(b1.getSchedulesAller().get(s).get(i))-minutesSinceMidnight));
                }
            }
        }
        roadInfoAdapter = new RoadInfoAdapter(this,roadList);
        ListView listRoads = findViewById(R.id.listRoads);
        listRoads.setAdapter(roadInfoAdapter);


    }

    private int convertScheduleToInt(String schedule){
        String[] parts = schedule.split("h");
        int hours = Integer.parseInt(parts[0]);
        int minutes = Integer.parseInt(parts[1]);

        // Convertir les heures en minutes et ajouter les minutes
        return hours * 60 + minutes;
    }
}
