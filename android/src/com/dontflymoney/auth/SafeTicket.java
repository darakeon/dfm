package com.dontflymoney.auth;

import java.util.Locale;

import android.content.Context;
import android.provider.Settings.Secure;

class SafeTicket
{
	private String key;
	
	public SafeTicket(Context context)
	{
		String machineId = Secure.getString(context.getContentResolver(), Unique.GetKey());
		
		long factor = Long.parseLong(machineId, 16) * 27;
		key = Long.toHexString(factor);
		key = key.toUpperCase(Locale.ITALY);

		key += key;
		key += key;
	}
	
	
	
	public String Encrypt(String ticket)
	{	
		if (ticket == null)
			return null;
		
		String encryptedTicket = "";
		
		for(int s = 0; s < ticket.length(); s++)
		{
			encryptedTicket +=
				key.substring(s, s+1)
				+ ticket.substring(s, s+1);
		}
		
		return encryptedTicket;
	}

	public String Decrypt(String encryptedTicket)
	{
		if (encryptedTicket == null)
			return null;
		
		String ticket = "";
		
		for(int s = 0; s < encryptedTicket.length(); s+=2)
		{
			String keyChar = key.substring(s/2, s/2+1); 
			String encryptedChar = encryptedTicket.substring(s, s+1);
			
			if (!keyChar.equals(encryptedChar))
			{
				ticket = null;
				break;
			}
			
			ticket += encryptedTicket.substring(s+1, s+2);
		}
		
		return ticket;

	}

	

}
