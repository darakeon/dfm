package com.dontflymoney.auth;

import android.content.Context;

import com.dontflymoney.io.File;
import com.dontflymoney.io.FileNames;

public class Authentication
{
	private Context context;
	private SafeTicket safe;
	
	public Authentication(Context context)
	{
		this.context = context;
		safe = new SafeTicket(this.context);
	}
	
	public void Set(String ticket)
	{
		File ticketFile = new File(context, FileNames.Ticket);
		
		String encryptedTicket = safe.Encrypt(ticket);
		
		ticketFile.WriteToFile(encryptedTicket);
	}
	
	public String Get()
	{
		File ticketFile = new File(context, FileNames.Ticket);
		
		String encryptedTicket = ticketFile.ReadFromFile();
		
		return safe.Decrypt(encryptedTicket);
	}

	public boolean IsLoggedIn()
	{
		String ticket = Get();

		return ticket != null && !ticket.isEmpty();
	}

	public void Clear()
	{
		Set(null);
	}
	

}