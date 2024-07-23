package com.example.myapplication;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.PopupWindow;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

public class MainActivity extends AppCompatActivity {
    private boolean english = true;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        ImageView homeImg = findViewById(R.id.homeButton);
        homeImg.setImageResource(R.drawable.home_on);
        TextView hometxt = findViewById(R.id.textHomebutton);
        hometxt.setTextColor(Color.parseColor("#FFC656"));
        findViewById(R.id.settingsPopup).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showPopupInitial(v);
            }
        });
        findViewById(R.id.schedulesButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, SchedulesActivity.class);
                startActivity(intent);

            }
        });
    }

    private void showPopupInitial(View anchorView) {
        LayoutInflater inflater = (LayoutInflater) getSystemService(LAYOUT_INFLATER_SERVICE);
        View popupView = inflater.inflate(R.layout.popup_parametres, null);


        int width = 700;
        int height = LinearLayout.LayoutParams.WRAP_CONTENT;
        boolean focusable = true; // Lets taps outside the popup also dismiss it
        final PopupWindow popupWindow = new PopupWindow(popupView, width, height, focusable);
        if (english){
            ImageView img = popupView.findViewById(R.id.flag_uk);
            img.setBackgroundResource(R.drawable.bordureverte);

        }else{
            ImageView img = popupView.findViewById(R.id.flag_vn);
            img.setBackgroundResource(R.drawable.bordureverte);

        }
        popupWindow.showAtLocation(anchorView.getRootView(), Gravity.CENTER, 0, 0);

        popupView.findViewById(R.id.exitParameters).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Handle profile click
                popupWindow.dismiss();
            }
        });

        // Set up click listeners for each menu item
        popupView.findViewById(R.id.flag_uk).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Handle flag UK click
                ImageView img = popupView.findViewById(R.id.flag_uk);
                img.setBackgroundResource(R.drawable.bordureverte);
                ImageView img2 = popupView.findViewById(R.id.flag_vn);
                img2.setBackgroundResource(R.drawable.nobordure);
                english = true;
                //TODO changer la langue
            }
        });

        popupView.findViewById(R.id.flag_vn).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Handle flag VN click
                ImageView img = popupView.findViewById(R.id.flag_uk);
               img.setBackgroundResource(R.drawable.nobordure);
                ImageView img2 = popupView.findViewById(R.id.flag_vn);
                img2.setBackgroundResource(R.drawable.bordureverte);
                english=false;
                //TODO changer la langue
            }
        });

        popupView.findViewById(R.id.settingsBTN).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Handle settings click
                popupWindow.dismiss();
                showPopupSettings(v);
            }
        });

        popupView.findViewById(R.id.profileBTN).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Handle profile click
                popupWindow.dismiss();
                showPopupProfile(v);
            }
        });

        popupView.findViewById(R.id.helpBTN).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Handle help click
            }
        });

        popupView.findViewById(R.id.exitBTN).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                finishAffinity();
                System.exit(0);
            }
        });
    }
    private void showPopupSettings(View anchorView) {
        LayoutInflater inflater = (LayoutInflater) getSystemService(LAYOUT_INFLATER_SERVICE);
        View popupView = inflater.inflate(R.layout.popup_settings, null);


        int width = 700;
        int height = LinearLayout.LayoutParams.WRAP_CONTENT;
        boolean focusable = true; // Lets taps outside the popup also dismiss it
        final PopupWindow popupWindow = new PopupWindow(popupView, width, height, focusable);


        popupWindow.showAtLocation(anchorView.getRootView(), Gravity.CENTER, 0, 0);

        popupView.findViewById(R.id.exitParameters).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Handle profile click
                popupWindow.dismiss();
                showPopupInitial(v);
            }
        });


        popupView.findViewById(R.id.notifBTN).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Handle profile click
            }
        });

        popupView.findViewById(R.id.soundsBTN).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Handle help click
            }
        });


    }
    private void showPopupProfile(View anchorView) {
        LayoutInflater inflater = (LayoutInflater) getSystemService(LAYOUT_INFLATER_SERVICE);
        View popupView = inflater.inflate(R.layout.popup_profile, null);


        int width = 700;
        int height = LinearLayout.LayoutParams.WRAP_CONTENT;
        boolean focusable = true; // Lets taps outside the popup also dismiss it
        final PopupWindow popupWindow = new PopupWindow(popupView, width, height, focusable);


        popupWindow.showAtLocation(anchorView.getRootView(), Gravity.CENTER, 0, 0);

        popupView.findViewById(R.id.exitParameters).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Handle profile click
                popupWindow.dismiss();
                showPopupInitial(v);
            }
        });


        popupView.findViewById(R.id.connectBTN).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Handle profile click
                popupWindow.dismiss();
                //TODO connexion
            }
        });

        popupView.findViewById(R.id.editPassword).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Handle help click
            }
        });
        popupView.findViewById(R.id.editUsername).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Handle help click
            }
        });

    }
}