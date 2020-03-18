package com.darakeon.dfm.utils.robolectric

import android.app.Activity
import android.content.Context
import android.net.ConnectivityManager
import android.net.NetworkCapabilities
import org.robolectric.Shadows.shadowOf

fun Activity.simulateNetwork() {
	val connectivityManager =
		getSystemService(Context.CONNECTIVITY_SERVICE)
			as ConnectivityManager

	val shadowManager = shadowOf(connectivityManager)

	val network = connectivityManager.activeNetwork
	val capabilities = NetworkCapabilities(null)
	val shadowCapabilities = shadowOf(capabilities)
	shadowCapabilities.addTransportType(
		NetworkCapabilities.TRANSPORT_WIFI
	)
	shadowManager.setNetworkCapabilities(
		network, capabilities
	)
}
