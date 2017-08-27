package com.dontflymoney.io;

public enum FileNames
{
	Ticket("ticket" + File.extension);
	
	String name;
	
	FileNames(String name)
	{
		this.name = name;
	}
	
	String getName()
	{
		return name;
	}
}