package com.dontflymoney.stati;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;

public class Internet
{
	public static boolean isOffline(Context context)
	{
		ConnectivityManager conMgr =  
			(ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);

		return conMgr.getNetworkInfo(0).getState() != NetworkInfo.State.CONNECTED 
			    &&  conMgr.getNetworkInfo(1).getState() != NetworkInfo.State.CONNECTED;
	}
}
