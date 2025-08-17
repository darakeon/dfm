package com.darakeon.dfm.lib.api

import android.content.Context
import android.net.ConnectivityManager
import android.net.NetworkCapabilities
import android.os.Build.TAGS
import android.os.Build.TYPE

class Internet(context: Context) {
	companion object {
		fun isEmulator() =
			TAGS == "dev-keys" && TYPE == "userdebug"
		// apk > cellphone: TAGS == "release-keys" && TYPE = "user"
	}

	private val connectivityManager =
		context.getSystemService(
			Context.CONNECTIVITY_SERVICE
		) as ConnectivityManager?

	fun isOffline() = !isOnline()

	private fun isOnline() : Boolean {
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
