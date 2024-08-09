package com.example.myapplication.Activity;

import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.view.View;
import android.widget.ListView;
import android.widget.TextView;

import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;

import com.example.myapplication.Adapter.RoadInfoAdapter;
import com.example.myapplication.Adapter.SearchRoadAdapter;
import com.example.myapplication.Item.BusStopItem;
import com.example.myapplication.Item.RoadInfoItem;
import com.example.myapplication.R;

import java.time.LocalTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class SearchRoadActivity extends AppCompatActivity {

    private ArrayList<BusStopItem> busStopItems = new ArrayList<>();
    private SearchRoadAdapter searchRoadAdapter;
    private RoadInfoAdapter roadInfoAdapter;
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.search_road_layout);
        createLists();
        ListView listRoads = findViewById(R.id.listRoads);
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
    }
    private void createLists(){
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
        for(int i = 0;i<listDepart.size();i++){
            busStopItems.add(new BusStopItem(listDepart.get(i),"Danang,Vietnam",hashMap));
        }

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
                            minutesSinceMidnight-convertScheduleToInt(b1.getSchedulesAller().get(s).get(i))));
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
