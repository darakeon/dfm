package com.dontflymoney.userdata;

import android.content.Context;

public class Authentication
{
	private Context context;
	private SafeTicket safe;

	private static final String spKey = "Ticket";



	public Authentication(Context context)
	{
		this.context = context;
		safe = new SafeTicket(this.context);
	}
	
	public void Set(String ticket)
	{
		String encryptedTicket = safe.Encrypt(ticket);
		SP.setValue(context, spKey, encryptedTicket);
	}
	
	public String Get()
	{
		String encryptedTicket = SP.getValue(context, spKey);
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