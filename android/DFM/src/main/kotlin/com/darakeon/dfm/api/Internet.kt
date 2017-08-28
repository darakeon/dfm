package com.darakeon.dfm.api

import android.content.Context
import android.net.ConnectivityManager
import android.net.NetworkInfo

internal object Internet {
    fun isOffline(context: Context): Boolean {
        val conMgr = context.getSystemService(Context.CONNECTIVITY_SERVICE) as ConnectivityManager

        val networkInfo = conMgr.activeNetworkInfo

        val noNetwork = networkInfo == null || networkInfo.state != NetworkInfo.State.CONNECTED

        return noNetwork
    }
}
