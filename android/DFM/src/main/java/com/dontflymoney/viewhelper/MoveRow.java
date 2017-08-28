package com.dontflymoney.viewhelper;

import android.content.Context;
import android.content.res.Configuration;
import android.view.Gravity;
import android.view.View;
import android.widget.TableRow;
import android.widget.TextView;

import com.dontflymoney.view.ExtractActivity;
import com.dontflymoney.view.R;

import org.json.JSONException;
import org.json.JSONObject;

import java.text.DateFormat;
import java.util.Calendar;

public class MoveRow extends TableRow
{
    private final ExtractActivity activity;
    private String description;
    //private double total;
    private boolean checked;

    public MoveRow(Context context)
    {
        super(context);
        activity = (ExtractActivity)context;

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
        addView(activity.form.createText(description, Gravity.START));
    }

    public String getDescription()
    {
        return description;
    }

    public void setDate(JSONObject date) throws JSONException
    {
        if (getResources().getConfiguration().orientation != Configuration.ORIENTATION_LANDSCAPE)
            return;

        Calendar calendar = DateTime.getCalendar(date);

        DateFormat format = DateFormat.getDateInstance(DateFormat.SHORT);

        TextView cell = activity.form.createText(format.format(calendar.getTime()), Gravity.CENTER);
        cell.setPadding(20, 20, 200, 20);

        addView(cell);
    }

    public void setTotal(double total)
    {
        //this.total = total;
        addView(activity.form.createText(total, Gravity.END));
    }

    public void setChecked(Boolean checked)
    {
        this.checked = checked;

		if (activity.CanCheck())
		{
			int idResource = checked ? R.drawable.green_sign : R.drawable.red_sign;
			addView(activity.form.createImage(idResource));
		}
    }

	public Boolean getChecked()
	{
		return checked;
	}


}
