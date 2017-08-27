package com.dontflymoney.auth;

import android.content.Context;

public class Authentication
{
	public static void Set(Context context, String ticket)
	{
		File ticketFile = new File(context, File.Ticket);
		
		ticketFile.WriteToFile(ticket);
	}
	
	public static String Get(Context context)
	{
		File ticketFile = new File(context, File.Ticket);
		
		return ticketFile.ReadFromFile();
	}

	public static boolean IsLoggedIn(Context context)
	{
		String ticket = Get(context);

		return ticket != null && ticket != "";
	}

	public static void Clear(Context context)
	{
		Set(context, null);
	}
	

}