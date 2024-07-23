    package com.example.myapplication;

    import android.content.Context;
    import android.view.LayoutInflater;
    import android.view.View;
    import android.view.ViewGroup;
    import android.widget.ArrayAdapter;
    import android.widget.ImageView;
    import android.widget.TextView;

    import androidx.annotation.NonNull;
    import androidx.annotation.Nullable;

    import org.w3c.dom.Text;

    import java.util.ArrayList;

    public class LineListAdapter extends ArrayAdapter<LineList> {
        public LineListAdapter(@NonNull Context context, ArrayList<LineList> dataArrayList) {
            super(context, R.layout.listview_line,dataArrayList);
        }

        @NonNull
        @Override
        public View getView(int position, @Nullable View view, @NonNull ViewGroup parent) {
            LineList lineList = getItem(position);

            if(view == null){
                view = LayoutInflater.from(getContext()).inflate(R.layout.listview_line,parent,false);
            }
            ImageView listImage = view.findViewById(R.id.nbLine);
            TextView departTxt = view.findViewById(R.id.departLineTxt);
            TextView arriveeTxt = view.findViewById(R.id.arriveeLineTxt);

            listImage.setImageResource(lineList.getImage());
            departTxt.setText("from: " + lineList.getDepart());
            arriveeTxt.setText("to: " + lineList.getArrivee());

            ImageView favImage = view.findViewById(R.id.favoriteBTN);
            if(lineList.isFavorite())
                favImage.setImageResource(R.drawable.favorite_on);
            else
                favImage.setImageResource(R.drawable.favorite_off);
            favImage.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    System.out.println(lineList.getDepart());
                    lineList.setFavorite(!lineList.isFavorite());
                    if(lineList.isFavorite())
                        favImage.setImageResource(R.drawable.favorite_on);
                    else
                        favImage.setImageResource(R.drawable.favorite_off);
                }
            });

            return view;
        }
    }
