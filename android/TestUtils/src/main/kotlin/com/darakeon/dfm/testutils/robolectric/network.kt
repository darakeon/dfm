package com.darakeon.dfm.testutils.robolectric

import android.app.Activity
import android.content.Context
import android.net.ConnectivityManager
import android.net.NetworkCapabilities
import android.net.NetworkCapabilities.TRANSPORT_CELLULAR
import android.net.NetworkCapabilities.TRANSPORT_ETHERNET
import android.net.NetworkCapabilities.TRANSPORT_WIFI
import android.os.Build
import androidx.annotation.RequiresApi
import org.robolectric.Shadows.shadowOf
import org.robolectric.shadows.ShadowNetworkCapabilities

@RequiresApi(Build.VERSION_CODES.M)
fun Activity.simulateNetwork() {
	simulateState {
		it.addTransportType(TRANSPORT_WIFI)
	}
}

@RequiresApi(Build.VERSION_CODES.M)
fun Activity.simulateOffline() {
	simulateState {
		it.removeTransportType(TRANSPORT_WIFI)
		it.removeTransportType(TRANSPORT_ETHERNET)
		it.removeTransportType(TRANSPORT_CELLULAR)
	}
}

@RequiresApi(Build.VERSION_CODES.M)
private fun Activity.simulateState(change: (ShadowNetworkCapabilities) -> Unit) {
	val connectivityManager =
		getSystemService(Context.CONNECTIVITY_SERVICE)
			as ConnectivityManager

	val shadowManager = shadowOf(connectivityManager)

	val network = connectivityManager.activeNetwork
	val capabilities = NetworkCapabilities(null)
	val shadowCapabilities = shadowOf(capabilities)

	change(shadowCapabilities)

	shadowManager.setNetworkCapabilities(
		network, capabilities
	)
}
