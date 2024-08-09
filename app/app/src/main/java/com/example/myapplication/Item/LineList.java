package com.example.myapplication.Item;

import android.os.Parcel;
import android.os.Parcelable;

import androidx.annotation.NonNull;

public class LineList implements Parcelable {

    private int image;
    private String nbLine;
    private String depart;
    private String arrivee;
    private boolean favorite = false;

    public LineList(int image,String nbLine, String depart, String arrivee) {
        this.image = image;
        this.nbLine = nbLine;
        this.depart = depart;
        this.arrivee = arrivee;
    }


    protected LineList(Parcel in) {
        image = in.readInt();
        nbLine = in.readString();
        depart = in.readString();
        arrivee = in.readString();
        favorite = in.readByte() != 0;
    }

    public static final Creator<LineList> CREATOR = new Creator<LineList>() {
        @Override
        public LineList createFromParcel(Parcel in) {
            return new LineList(in);
        }

        @Override
        public LineList[] newArray(int size) {
            return new LineList[size];
        }
    };

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


    public String getNbLine() {
        return nbLine;
    }

    public void setNbLine(String nbLine) {
        this.nbLine = nbLine;
    }

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(@NonNull Parcel dest, int flags) {
        dest.writeInt(image);
        dest.writeString(nbLine);
        dest.writeString(depart);
        dest.writeString(arrivee);
        dest.writeByte((byte) (favorite ? 1 : 0));
    }
}
