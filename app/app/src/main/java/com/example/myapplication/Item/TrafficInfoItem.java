package com.example.myapplication.Item;

import java.util.List;

public class TrafficInfoItem {

    private String title;
    private String descrition;
    private List<String> numbers;

    public TrafficInfoItem(String title,String description, List<String> numbers) {
        this.title = title;
        this.descrition = description;
        this.numbers = numbers;
    }

    public String getDescription() {
        return descrition;
    }

    public String getTitle() {
        return title;
    }

    public List<String> getNumbers() {
        return numbers;
    }

}
