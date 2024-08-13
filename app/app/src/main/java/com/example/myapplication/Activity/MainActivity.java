package com.example.myapplication.Activity;

import android.Manifest;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.location.Location;
import android.os.Bundle;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.FrameLayout;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.PopupWindow;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentActivity;

import com.example.myapplication.R;
import com.google.android.gms.location.FusedLocationProviderClient;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.maps.CameraUpdate;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.MapView;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.BitmapDescriptor;
import com.google.android.gms.maps.model.BitmapDescriptorFactory;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MapStyleOptions;
import com.google.android.gms.maps.model.MarkerOptions;
import com.google.android.gms.tasks.OnSuccessListener;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

public class MainActivity extends FragmentActivity implements OnMapReadyCallback {
    private boolean english = true;
    FrameLayout mapView;
    GoogleMap gMap;
    LatLng userLocation;
    private FusedLocationProviderClient fusedLocationClient;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        fusedLocationClient = LocationServices.getFusedLocationProviderClient(this);

        if (ContextCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION)
                != PackageManager.PERMISSION_GRANTED) {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, 1);
        } else {
            // Permission already granted, get the user's location
            getUserLocation();
        }
        ImageView homeImg = findViewById(R.id.homeButton);
        homeImg.setImageResource(R.drawable.home_on);
        TextView hometxt = findViewById(R.id.textHomebutton);
        hometxt.setTextColor(Color.parseColor("#FFC656"));

        mapView = findViewById(R.id.map);

        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager().findFragmentById(R.id.map);
        mapFragment.getMapAsync(this);

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
        findViewById(R.id.trafficInfoButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, TrafficInfoActivity.class);
                startActivity(intent);

            }
        });
        findViewById(R.id.searchRoadBTN).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, SearchRoadActivity.class);
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
        if (english) {
            ImageView img = popupView.findViewById(R.id.flag_uk);
            img.setBackgroundResource(R.drawable.bordureverte);

        } else {
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
                english = false;
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

    private void getUserLocation() {
        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION)
                != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_COARSE_LOCATION)
                != PackageManager.PERMISSION_GRANTED) {
            return;
        }
        fusedLocationClient.getLastLocation()
                .addOnSuccessListener(this, new OnSuccessListener<Location>() {
                    @Override
                    public void onSuccess(Location location) {
                        // Got last known location. In some rare situations this can be null.
                        if (location != null) {
                            userLocation = new LatLng(location.getLatitude(), location.getLongitude());
                            gMap.moveCamera(CameraUpdateFactory.newLatLngZoom(userLocation, 16));


                            MarkerOptions markerOptions = new MarkerOptions()
                                    .position(userLocation)
                                    .icon(bitmapDescriptorFromVector(MainActivity.this, R.drawable.custom_marker));
                            gMap.addMarker(markerOptions);
                            retrieveWeather();
                        }
                    }
                });
    }

    private BitmapDescriptor bitmapDescriptorFromVector(Context context, int vectorResId) {
        Drawable vectorDrawable = ContextCompat.getDrawable(context, vectorResId);
        vectorDrawable.setBounds(0, 0, vectorDrawable.getIntrinsicWidth(), vectorDrawable.getIntrinsicHeight());
        Bitmap bitmap = Bitmap.createBitmap(vectorDrawable.getIntrinsicWidth(), vectorDrawable.getIntrinsicHeight(), Bitmap.Config.ARGB_8888);
        Canvas canvas = new Canvas(bitmap);
        vectorDrawable.draw(canvas);
        return BitmapDescriptorFactory.fromBitmap(bitmap);
    }

    @Override
    public void onMapReady(@NonNull GoogleMap googleMap) {
        this.gMap = googleMap;
        googleMap.setMapType(GoogleMap.MAP_TYPE_NORMAL);

        //googleMap.setMapType(GoogleMap.MAP_TYPE_NONE);
        if (ContextCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION)
                == PackageManager.PERMISSION_GRANTED) {
            gMap.setMyLocationEnabled(true);
            getUserLocation();
        } else {
            // If permission is not granted, request it
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, 1);
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == 1) {
            if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                if (ContextCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION)
                        == PackageManager.PERMISSION_GRANTED) {
                    gMap.setMyLocationEnabled(true);
                    getUserLocation();
                }
            } else {
                // Permission denied
            }
        }
    }

    private void retrieveWeather() {
        new Thread(new Runnable() {
            @Override
            public void run() {
                String lat = ""+userLocation.latitude;
                String lon = ""+userLocation.longitude;
                String apiKey = "d8cf44c10cf0a4aa3d330939e8562642";
                String apiUrl = "https://api.openweathermap.org/data/2.5/weather?lat="+lat+"&lon="+lon+"&appid="+apiKey;
                try {
                    URL url = new URL(apiUrl);
                    HttpURLConnection conn = (HttpURLConnection) url.openConnection();
                    conn.setRequestMethod("GET");

                    BufferedReader in = new BufferedReader(new InputStreamReader(conn.getInputStream()));
                    String inputLine;
                    StringBuilder response = new StringBuilder();

                    while ((inputLine = in.readLine()) != null) {
                        response.append(inputLine);
                    }
                    in.close();

                    // Parser la réponse JSON
                    JSONObject jsonResponse = new JSONObject(response.toString());
                    JSONArray weatherArray = jsonResponse.getJSONArray("weather");
                    JSONObject weatherObject = weatherArray.getJSONObject(0);
                    String weatherMain = weatherObject.getString("main").toLowerCase();

                    JSONObject mainObject = jsonResponse.getJSONObject("main");
                    int temp = (int) (mainObject.getDouble("temp") - 273.15);

                    // Afficher les informations
                    System.out.println("Weather Main: " + weatherMain);
                    ImageView logoWeather = findViewById(R.id.circleIcon);
                    if(weatherMain.contains("clouds")){
                        logoWeather.setImageResource(R.drawable.cloudy);
                    }else if(weatherMain.contains("clear")){
                        logoWeather.setImageResource(R.drawable.sunny);
                    }else if(weatherMain.contains("snow")){
                        logoWeather.setImageResource(R.drawable.snow);
                    }else if(weatherMain.contains("thunder")){
                        logoWeather.setImageResource(R.drawable.thunder);
                    }else if(weatherMain.contains("mist")){
                        logoWeather.setImageResource(R.drawable.mist);
                    }else {
                        logoWeather.setImageResource(R.drawable.rainy);
                    }
                    TextView tempTxt = findViewById(R.id.temperature);
                    tempTxt.setText(temp +" °C");


                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }).start();
    }

}


