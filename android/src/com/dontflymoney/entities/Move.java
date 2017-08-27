package com.dontflymoney.entities;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;

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
	
	public void setDay(int day)
	{
		Date.set(Calendar.DAY_OF_MONTH, day);
	}
	
	public void setMonth(int month)
	{
		Date.set(Calendar.MONTH, month);
	}
	
	public void setYear(int year)
	{
		Date.set(Calendar.YEAR, year);
	}
	
	
	
	
	
}
