package com.dontflymoney.io;

public enum FileNames
{
	Ticket("ticket"),
	Language("language");
	
	String name;
	
	FileNames(String name)
	{
		this.name = name + File.extension;
	}
	
	String getName()
	{
		return name;
	}
}