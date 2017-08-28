package com.dontflymoney.userdata;

import android.content.Context;
import android.content.SharedPreferences;

class SP
{
	private static final String sharedPreferencesDFM = "DfM";

	public static String getValue(Context context, String key)
	{
		SharedPreferences sp = context.getSharedPreferences(sharedPreferencesDFM, Context.MODE_PRIVATE);
		return sp.getString(key, null);
	}

	public static void setValue(Context context, String key, String value)
	{
		SharedPreferences sp = context.getSharedPreferences(sharedPreferencesDFM, Context.MODE_PRIVATE);
		SharedPreferences.Editor edit = sp.edit();

		edit.putString(key, value);
		edit.apply();
	}
}
