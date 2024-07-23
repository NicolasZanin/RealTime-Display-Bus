package com.example.myapplication;

import android.media.Image;

public class LineList {

    private int image;
    private String depart;
    private String arrivee;
    private boolean favorite = false;

    public LineList(int image, String depart, String arrivee) {
        this.image = image;
        this.depart = depart;
        this.arrivee = arrivee;
    }

    public int getImage() {
        return image;
    }

    public void setImage(int image) {
        this.image = image;
    }

    public String getDepart() {
        return depart;
    }

    public void setDepart(String depart) {
        this.depart = depart;
    }

    public String getArrivee() {
        return arrivee;
    }

    public void setArrivee(String arrivee) {
        this.arrivee = arrivee;
    }

    public boolean isFavorite() {
        return favorite;
    }

    public void setFavorite(boolean favorite) {
        this.favorite = favorite;
    }
}
