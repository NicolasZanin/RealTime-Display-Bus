package com.example.myapplication.Item;

public class RoadInfoItem {
    private String line;
    private String departure;
    private String arrival;
    private int timeLeft;

    public RoadInfoItem(String line,String departure, String arrival, int timeLeft) {
        this.line = line;
        this.departure = departure;
        this.arrival = arrival;
        this.timeLeft = timeLeft;
    }

    public String getDeparture() {
        return departure;
    }

    public void setDeparture(String departure) {
        this.departure = departure;
    }

    public String getArrival() {
        return arrival;
    }

    public void setArrival(String arrival) {
        this.arrival = arrival;
    }

    public int getTimeLeft() {
        return timeLeft;
    }

    public void setTimeLeft(int timeLeft) {
        this.timeLeft = timeLeft;
    }

    public String getLine() {
        return line;
    }

    public void setLine(String line) {
        this.line = line;
    }
}
