package com.dontflymoney.auth;

import android.provider.Settings.Secure;

public class Unique
{
	public static String GetKey()
	{
		return Secure.ANDROID_ID;
	}
	
}
