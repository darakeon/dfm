package com.dontflymoney.entities;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.Locale;

import com.dontflymoney.api.Request;

public class Move
{
	public Move()
	{
		Details = new ArrayList<Detail>();
		Date = Calendar.getInstance();
	}
	
	public String Description;
    public Calendar Date;
    public String Category;
    public String PrimaryAccount;
    public String OtherAccount;
    public int Nature;
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
		request.AddParameter("Description", Description);
		request.AddParameter("Date", DateString());
		request.AddParameter("Category", Category);
		request.AddParameter("PrimaryAccount", PrimaryAccount);
		request.AddParameter("OtherAccount", OtherAccount);
		request.AddParameter("Nature", Nature);
		request.AddParameter("Value", Value);
		
		for(Detail detail : Details)
		{
			int position = Details.lastIndexOf(detail);
			
			request.AddParameter("DetailList[" + position + "].Description", detail.Description);
			request.AddParameter("DetailList[" + position + "].Amount", detail.Amount);
			request.AddParameter("DetailList[" + position + "].Value", detail.Value);
		}		
	}

	public String DateString() {
		SimpleDateFormat formatter = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
		
		return formatter.format(Date.getTime());
	}
	
	
	
	
	
}
