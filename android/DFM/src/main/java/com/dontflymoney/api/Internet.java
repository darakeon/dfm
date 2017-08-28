package com.dontflymoney.api;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;

public class Internet
{
	public static boolean isOffline(Context context)
	{
		ConnectivityManager conMgr = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);

		NetworkInfo networkInfoWifi = conMgr.getNetworkInfo(0);
		NetworkInfo networkInfo3G = conMgr.getNetworkInfo(1);

		Boolean noWifi = networkInfoWifi == null || networkInfoWifi.getState() != NetworkInfo.State.CONNECTED;
		Boolean no3G = networkInfo3G == null || networkInfo3G.getState() != NetworkInfo.State.CONNECTED;

		return noWifi && no3G;
	}
}
