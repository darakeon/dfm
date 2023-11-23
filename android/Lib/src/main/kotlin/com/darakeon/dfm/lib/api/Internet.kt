package com.darakeon.dfm.lib.api

import android.content.Context
import android.net.ConnectivityManager
import android.net.NetworkCapabilities
import android.os.Build
import android.os.Build.TAGS
import android.os.Build.TYPE
import androidx.annotation.RequiresApi

abstract class BaseInternet(context: Context) {
	private val connectivityManager =
		context.getSystemService(
			Context.CONNECTIVITY_SERVICE
		) as ConnectivityManager?

	fun isOffline() = !isOnline(connectivityManager)

	protected abstract fun isOnline(
		connectivityManager: ConnectivityManager?
	) : Boolean
}

@RequiresApi(Build.VERSION_CODES.M)
class InternetNew(context: Context) : BaseInternet(context) {
	override fun isOnline(connectivityManager: ConnectivityManager?): Boolean {
		return testConnection(connectivityManager) {
			it.hasTransport(NetworkCapabilities.TRANSPORT_WIFI)
			|| it.hasTransport(NetworkCapabilities.TRANSPORT_CELLULAR)
			|| it.hasTransport(NetworkCapabilities.TRANSPORT_ETHERNET)
		}
	}

	private fun testConnection(
		connectivityManager: ConnectivityManager?,
	   test: (NetworkCapabilities) -> Boolean
	): Boolean {
		val network =
			connectivityManager?.activeNetwork
				?: return false

		val capabilities =
			connectivityManager.getNetworkCapabilities(network)
				?: return false

		return test(capabilities)
	}
}

class InternetOld(context: Context) : BaseInternet(context) {
	@Suppress("DEPRECATION")
	override fun isOnline(connectivityManager: ConnectivityManager?) : Boolean {
		var result = false

		connectivityManager.run {
			connectivityManager?.activeNetworkInfo?.run {
				result = state == android.net.NetworkInfo.State.CONNECTED
			}
		}

		return result
	}
}

object Internet {
	private fun internet(context: Context) =
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M)
			InternetNew(context)
		else
			InternetOld(context)

	fun isOffline(context: Context) =
		internet(context).isOffline()

	fun isEmulator() =
		TAGS == "dev-keys" && TYPE == "userdebug"
}
