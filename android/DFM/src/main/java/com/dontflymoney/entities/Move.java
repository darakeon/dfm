package com.dontflymoney.entities;

import com.dontflymoney.api.Request;
import com.dontflymoney.viewhelper.DateTime;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.Locale;

public class Move
{
	public Move()
	{
		Details = new ArrayList<>();
		Date = Calendar.getInstance();
	}

    public int Id;
	public String Description;
    public Calendar Date;
    public String Category;
    public String PrimaryAccount;
    public String OtherAccount;
    public int Nature;

    public boolean isDetailed;
    public double Value;
    public List<Detail> Details;



    public void Add(String description, int amount, double value)
    {
    	Detail detail = new Detail();
    	
    	detail.Description = description;
    	detail.Amount = amount;
    	detail.Value = value;
    	
    	Details.add(detail);
    }

	public void Remove(String description, int amount, double value)
	{
		for (Detail detail : Details)
		{
			if (detail.Equals(description, amount, value))
			{
				Details.remove(detail);
				return;
			}
		}
	}
	
	
	
	public int getDay()
	{
		return Date.get(Calendar.DAY_OF_MONTH);
	}
	
	public int getMonth()
	{
		return Date.get(Calendar.MONTH);
	}
	
	public int getYear()
	{
		return Date.get(Calendar.YEAR);
	}

	public void setParameters(Request request)
	{
        request.AddParameter("ID", Id);
        request.AddParameter("Description", Description);
        request.AddParameter("Date.Year", Date.get(Calendar.YEAR));
        request.AddParameter("Date.Month", Date.get(Calendar.MONTH) + 1);
        request.AddParameter("Date.Day", Date.get(Calendar.DAY_OF_MONTH));
		request.AddParameter("Category", Category);
		request.AddParameter("PrimaryAccount", PrimaryAccount);
		request.AddParameter("OtherAccount", OtherAccount);
		request.AddParameter("Nature", Nature);

        if (isDetailed)
        {
            for (Detail detail : Details)
            {
                int position = Details.lastIndexOf(detail);

                request.AddParameter("DetailList[" + position + "].Description", detail.Description);
                request.AddParameter("DetailList[" + position + "].Amount", detail.Amount);
                request.AddParameter("DetailList[" + position + "].Value", detail.Value);
            }
        }
        else
        {
            request.AddParameter("Value", Value);
        }
	}

	public String DateString() {
		SimpleDateFormat formatter = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());

		return formatter.format(Date.getTime());
	}




    public void SetData(JSONObject moveToEdit) throws JSONException
    {
        Id = moveToEdit.getInt("ID");
        Description = moveToEdit.getString("Description");
        Date = DateTime.getCalendar(moveToEdit.getJSONObject("Date"));
        Category = moveToEdit.getString("Category");
        OtherAccount = moveToEdit.getString("OtherAccount");
        Nature = moveToEdit.getInt("Nature");

        if (moveToEdit.has("Value") && !moveToEdit.get("Value").equals(null))
        {
            Value = moveToEdit.getDouble("Value");
        }

        if (moveToEdit.has("DetailList"))
        {
            JSONArray detailList = moveToEdit.getJSONArray("DetailList");

            isDetailed = detailList.length() > 0;

            for (int d = 0; d < detailList.length(); d++)
            {
                JSONObject detail = detailList.getJSONObject(d);
                Add(detail.getString("Description"), detail.getInt("Amount"), detail.getDouble("Value"));
            }
        }
    }
	
	
}
