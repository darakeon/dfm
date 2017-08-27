package com.dontflymoney.auth;

import android.content.Context;

public class Authentication
{
	Context context;
	SafeTicket safe;
	
	public Authentication(Context context)
	{
		this.context = context;
		safe = new SafeTicket(this.context);
	}
	
	public void Set(String ticket)
	{
		File ticketFile = new File(context, File.Ticket);
		
		String encryptedTicket = safe.Encrypt(ticket);
		
		ticketFile.WriteToFile(encryptedTicket);
	}
	
	public String Get()
	{
		File ticketFile = new File(context, File.Ticket);
		
		String encryptedTicket = ticketFile.ReadFromFile();
		
		return safe.Decrypt(encryptedTicket);
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