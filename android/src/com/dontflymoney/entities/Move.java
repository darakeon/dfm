package com.dontflymoney.entities;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class Move
{
	public Move()
	{
		Details = new ArrayList<Detail>();
	}
	
	public String Description;
    public Date Date;
    public String Category;
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
				break;
			}
		}
		
	}
    
}
