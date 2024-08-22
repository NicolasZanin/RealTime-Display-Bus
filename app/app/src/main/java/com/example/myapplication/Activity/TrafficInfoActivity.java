package com.example.myapplication.Activity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.myapplication.Adapter.TrafficInfoAdapter;
import com.example.myapplication.Item.DataBase;
import com.example.myapplication.R;
import com.example.myapplication.Item.TrafficInfoItem;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class TrafficInfoActivity extends AppCompatActivity {
private List<TrafficInfoItem> outerList = new ArrayList<>();
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.trafficinfo_layout);
        RecyclerView outerRecyclerView = findViewById(R.id.listTrafficInfo);
        outerRecyclerView.setLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false));
        
        outerList = DataBase.getInstance().getAlerts();
        TrafficInfoAdapter outerAdapter = new TrafficInfoAdapter(outerList);
        outerRecyclerView.setAdapter(outerAdapter);

        findViewById(R.id.homeButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(TrafficInfoActivity.this, MainActivity.class);
                startActivity(intent);

            }
        });
        findViewById(R.id.schedulesButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(TrafficInfoActivity.this, SchedulesActivity.class);

                startActivity(intent);

            }
        });



    }

}
