package com.dontflymoney.entities;

public class Detail
{
	public String Description;
	public int Amount;
	public double Value;
	
	boolean Equals(String description, int amount, double value)
	{
		return Description.equals(description)
			&& Amount == amount
			&& Value == value;
	}

}
