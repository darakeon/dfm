package com.dontflymoney.view;

import android.content.Context;
import android.content.res.Configuration;
import android.view.Gravity;
import android.view.View;
import android.widget.TableRow;
import android.widget.TextView;

import com.dontflymoney.baseactivity.SmartActivity;

import org.json.JSONException;
import org.json.JSONObject;

import java.text.DateFormat;
import java.util.Calendar;

public class MoveRow extends TableRow
{
    private final SmartActivity activity;
    private String description;
    private double total;

    public MoveRow(Context context)
    {
        super(context);
        activity = (SmartActivity)context;

        setOnClickListener(new OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                activity.clickedView = v;
                v.showContextMenu();
            }
        });
    }

    public int ID;

    public void setDescription(String description)
    {
        this.description = description;
        addView(activity.form.createText(description, Gravity.LEFT));
    }

    public String getDescription()
    {
        return description;
    }

    public void setDate(JSONObject date) throws JSONException
    {
        if (getResources().getConfiguration().orientation != Configuration.ORIENTATION_LANDSCAPE)
            return;

        Calendar calendar = Calendar.getInstance();
        calendar.set(Calendar.YEAR, date.getInt("Year"));
        calendar.set(Calendar.MONTH, date.getInt("Month") - 1);
        calendar.set(Calendar.DAY_OF_MONTH, date.getInt("Day"));

        DateFormat format = DateFormat.getDateInstance(DateFormat.SHORT);

        TextView cell = activity.form.createText(format.format(calendar.getTime()), Gravity.CENTER);
        cell.setPadding(20, 20, 200, 20);

        addView(cell);
    }

    public void setTotal(double total)
    {
        this.total = total;
        addView(activity.form.createText(total, Gravity.RIGHT));
    }
}
