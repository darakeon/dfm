package com.dontflymoney.auth;

import android.content.Context;
import android.provider.Settings.Secure;

public class Unique
{
	public static String GetKey(Context context)
	{
		return Secure.getString(context.getContentResolver(), Secure.ANDROID_ID);
	}
	
}
