package com.dontflymoney.auth;

import android.content.Context;

public class Authentication
{
	Context context;
	
	public Authentication(Context context)
	{
		this.context = context;
	}
	
	public void Set(String ticket)
	{
		File ticketFile = new File(context, File.Ticket);
		
		ticketFile.WriteToFile(ticket);
	}
	
	public String Get()
	{
		File ticketFile = new File(context, File.Ticket);
		
		return ticketFile.ReadFromFile();
	}

	public boolean IsLoggedIn()
	{
		String ticket = Get();

		return ticket != null && ticket != "";
	}

	public void Clear()
	{
		Set(null);
	}
	

}