package com.dontflymoney.api

import android.content.Context
import android.net.ConnectivityManager
import android.net.NetworkInfo

internal object Internet {
    fun isOffline(context: Context): Boolean {
        val conMgr = context.getSystemService(Context.CONNECTIVITY_SERVICE) as ConnectivityManager

        val networkInfoWifi = conMgr.getNetworkInfo(0)
        val networkInfo3G = conMgr.getNetworkInfo(1)

        val noWifi = networkInfoWifi == null || networkInfoWifi.state != NetworkInfo.State.CONNECTED
        val no3G = networkInfo3G == null || networkInfo3G.state != NetworkInfo.State.CONNECTED

        return noWifi && no3G
    }
}
