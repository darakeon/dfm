package com.dontflymoney.viewhelper;

import android.support.annotation.NonNull;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.Calendar;

/**
 * Created by Handhara on 07/09/2015.
 */
public class DateTime
{
    @NonNull
    public static Calendar getCalendar(JSONObject date) throws JSONException
    {
        Calendar calendar = Calendar.getInstance();
        calendar.set(Calendar.YEAR, date.getInt("Year"));
        calendar.set(Calendar.MONTH, date.getInt("Month") - 1);
        calendar.set(Calendar.DAY_OF_MONTH, date.getInt("Day"));

        return calendar;
    }
}
