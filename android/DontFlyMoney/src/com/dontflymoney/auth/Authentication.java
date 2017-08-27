package com.dontflymoney.auth;

import android.content.Context;

import com.dontflymoney.file.File;
import com.dontflymoney.file.FileNames;

public class Authentication
{
	public static void Set(Context context, String ticket)
	{
		File ticketFile = new File(context, FileNames.Ticket);
		
		ticketFile.WriteToFile(ticket);
	}
	
	public static String Get(Context context)
	{
		File ticketFile = new File(context, FileNames.Ticket);
		
		return ticketFile.ReadFromFile();
	}

	public static boolean IsLoggedIn(Context context)
	{
		String ticket = Get(context);

		return ticket != null && ticket != "";
	}
	

}
