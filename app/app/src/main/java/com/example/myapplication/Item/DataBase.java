package com.example.myapplication.Item;

import android.os.Environment;

import com.example.myapplication.R;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class DataBase {

    private static DataBase instance;
    List<TrafficInfoItem> alerts= new ArrayList<>();
    List<BusStopItem> stations = new ArrayList<>();
    List<LineList> lines = new ArrayList<>();
    private ExecutorService executorService = Executors.newSingleThreadExecutor();
    private boolean resRetour;
    private boolean resAller;
    public DataBase(){

    }

    public DataBase(List<BusStopItem> stations, List<LineList> lines) {
        this.stations = stations;
        this.lines = lines;
    }
    public static  DataBase getInstance() {
        if (instance == null) {
            instance = new DataBase();
        }
        return instance;
    }
    public boolean retrieveDataFromAPI() {

        executorService.execute(() -> {
            String urlString = "http://10.0.2.2:8000/api/Schedule/aller";

            try {
                URL url = new URL(urlString);
                HttpURLConnection conn = (HttpURLConnection) url.openConnection();
                conn.setRequestMethod("GET");
                conn.setRequestProperty("Accept-Charset", "UTF-8");

                int responseCode = conn.getResponseCode();
                if (responseCode == HttpURLConnection.HTTP_OK) {
                    BufferedReader in = new BufferedReader(new InputStreamReader(conn.getInputStream()));

                } else {
                    System.out.println("Erreur HTTP: " + responseCode);
                }
                BufferedReader in = new BufferedReader(new InputStreamReader(conn.getInputStream()));

                String inputLine;
                StringBuilder content = new StringBuilder();

                while ((inputLine = in.readLine()) != null) {
                    content.append(inputLine);
                }

                in.close();
                conn.disconnect();



                JSONArray jsonArray = new JSONArray(content.toString());

                String firstStop ="";
                String lastStop ="";
                for (int i = 0; i < jsonArray.length(); i++) {
                    JSONObject jsonObject = jsonArray.getJSONObject(i);


                    JSONArray schedulesArray = jsonObject.getJSONArray("schedules");
                    String name = jsonObject.getString("name");

                     List<String> schedules = new ArrayList<>();
                    for (int j = 0; j < schedulesArray.length(); j++) {
                        schedules.add(schedulesArray.getString(j));
                    }


                    HashMap<String,List<String>> tmp = new HashMap<>();
                    tmp.put("5",schedules);
                    stations.add(new BusStopItem(name,"Da Nang",tmp));
                    if(i==0){
                        firstStop = name;
                    }else if(i== (jsonArray.length()-1)){
                       lastStop = name;
                    }
                }

                createLines(firstStop,lastStop);
                resAller = true;
                retrieveSchedulesFromReturnAPI();
            } catch (Exception e) {
                e.printStackTrace();
                System.out.println("Erreur lors de la récupération des données : " + e.getMessage());
                resAller = false;
            }
        });
        createAlert();
        return resRetour && resAller;
    }
    private boolean retrieveSchedulesFromReturnAPI() {


        executorService.execute(() -> {
            String urlString = "http://10.0.2.2:8000/api/Schedule/retour";
            try {
                URL url = new URL(urlString);
                HttpURLConnection conn = (HttpURLConnection) url.openConnection();
                conn.setRequestMethod("GET");
                conn.setRequestProperty("Accept-Charset", "UTF-8");

                int responseCode = conn.getResponseCode();
                if (responseCode == HttpURLConnection.HTTP_OK) {
                    BufferedReader in = new BufferedReader(new InputStreamReader(conn.getInputStream()));
                    StringBuilder content = new StringBuilder();
                    String inputLine;

                    while ((inputLine = in.readLine()) != null) {
                        content.append(inputLine);
                    }

                    in.close();
                    conn.disconnect();

                    JSONArray jsonArray = new JSONArray(content.toString());


                    for (int i = 0; i < jsonArray.length(); i++) {
                        JSONObject jsonObject = jsonArray.getJSONObject(i);
                        JSONArray schedulesArray = jsonObject.getJSONArray("schedules");


                        List<String> schedules = new ArrayList<>();
                        for (int j = 0; j < schedulesArray.length(); j++) {
                            schedules.add(schedulesArray.getString(j));
                        }

                        String name = jsonObject.getString("name");
                        HashMap<String,List<String>> tmp = new HashMap<>();
                        tmp.put("5",schedules);
                        BusStopItem tmpB = new BusStopItem(name,"Da Nang");
                        if(stations.contains(tmpB)){
                            for(BusStopItem b : stations){
                                if(b.equals(tmpB)){
                                    b.setSchedulesRetour(tmp);
                                }
                            }
                        }else {
                            stations.add(new BusStopItem(name, "Da Nang", new HashMap<>(), tmp));
                        }

                    }


                } else {
                    System.out.println("Erreur HTTP pour retour API: " + responseCode);
                }
                resRetour= true;

            } catch (Exception e) {
                e.printStackTrace();
                System.out.println("Erreur lors de la récupération des données de retour : " + e.getMessage());
                resRetour = false;
            }
        });
        return resRetour;
    }

    public void createAlert(){
        alerts.add(new TrafficInfoItem("Alert Title 1","descrition Alert 1", Arrays.asList("1", "2", "3", "4", "5", "6")));
        alerts.add(new TrafficInfoItem("Alert Title 2","descrition Alert 2", Arrays.asList("1")));
        alerts.add(new TrafficInfoItem("Alert Title 3","descrition Alert 3", Arrays.asList("1", "2", "3", "4", "5", "6")));
        alerts.add(new TrafficInfoItem("Alert Title 4","descrition Alert 4", Arrays.asList("1", "5")));

    }

    public void createLines(String firstStop,String lastStop){
        LineList tmpLine = new LineList(R.drawable.line5,"5",firstStop,lastStop);
        lines.add(tmpLine);



        File directory = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOCUMENTS);
        File file = new File(directory, "favorite.txt");

        // Lire le fichier pour vérifier si le numéro de ligne est présent
        try {
            FileInputStream fis = new FileInputStream(file);
            InputStreamReader reader = new InputStreamReader(fis);
            BufferedReader bufferedReader = new BufferedReader(reader);

            String line;
            boolean found = false;
            while ((line = bufferedReader.readLine()) != null) {
                if (line.contains("|"+tmpLine.getNbLine()+"|")) { // Ici, vous pouvez ajuster le numéro de ligne comme nécessaire
                    found = true;
                    break;
                }
            }

            bufferedReader.close();
            reader.close();
            fis.close();

            if (found) {
                tmpLine.setFavorite(true);
            }

        } catch (IOException e) {
            e.printStackTrace();
        }
    }
    public List<TrafficInfoItem> getAlerts() {
        return alerts;
    }

    public void setAlerts(List<TrafficInfoItem> alerts) {
        this.alerts = alerts;
    }

    public List<BusStopItem> getStations() {
        return stations;
    }

    public void setStations(List<BusStopItem> stations) {
        this.stations = stations;
    }

    public List<LineList> getLines() {
        return lines;
    }

    public void setLines(List<LineList> lines) {
        this.lines = lines;
    }

    public boolean isDataRetrieve(){
        return resAller && resRetour;
    }


}
