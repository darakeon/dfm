package com.darakeon.dfm.testutils.robolectric

import android.app.Activity
import android.content.Context
import android.net.ConnectivityManager
import android.net.NetworkCapabilities
import android.os.Build
import androidx.annotation.RequiresApi
import org.robolectric.Shadows.shadowOf

@RequiresApi(Build.VERSION_CODES.M)
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
