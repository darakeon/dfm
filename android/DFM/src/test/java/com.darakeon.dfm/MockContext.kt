package com.darakeon.dfm

import android.content.Context
import android.content.res.Configuration
import android.content.res.Resources
import android.net.ConnectivityManager
import android.net.Network
import android.net.NetworkCapabilities
import android.os.Build
import com.darakeon.dfm.base.BaseActivity
import org.mockito.ArgumentMatchers.anyInt
import org.mockito.Mockito.`when`
import org.mockito.Mockito.mock

class MockContext {
	val activity: BaseActivity = mock(BaseActivity::class.java)
	private val manager = mock(ConnectivityManager::class.java)
	private var resourceCount = 0

	fun mockInternet(): MockContext {
		`when`(
			activity.getSystemService(
				Context.CONNECTIVITY_SERVICE
			)
		).thenReturn(manager)

		mockConnection(true)

		return this
	}

	private fun mockConnection(succeeded: Boolean) {
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M)
			mockConnectionNew(succeeded)
		else
			mockConnectionOld(succeeded)
	}

	private fun mockConnectionNew(hasTransport: Boolean) {
		val network = mock(Network::class.java)
		`when`(manager.activeNetwork).thenReturn(network)

		val capabilities = mock(NetworkCapabilities::class.java)
		`when`(manager.getNetworkCapabilities(network))
			.thenReturn(capabilities)

		`when`(capabilities.hasTransport(anyInt()))
			.thenReturn(hasTransport)
	}

	@Suppress("DEPRECATION")
	private fun mockConnectionOld(connected: Boolean) {
		val info = mock(android.net.NetworkInfo::class.java)
		`when`(manager.activeNetworkInfo).thenReturn(info)

		`when`(info.state).thenReturn(
			if (connected)
				android.net.NetworkInfo.State.CONNECTED
			else
				android.net.NetworkInfo.State.DISCONNECTED
		)
	}

	fun mockFailConnection() {
		mockConnection(false)
	}

	fun mockEmptyConnection() {
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M)
			mockEmptyConnectionNew()
		else
			mockEmptyConnectionOld()
	}

	private fun mockEmptyConnectionNew() {
		`when`(manager.activeNetwork).thenReturn(null)
	}

	@Suppress("DEPRECATION")
	private fun mockEmptyConnectionOld() {
		`when`(manager.activeNetworkInfo).thenReturn(null)
	}

	fun mockResources(): MockContext {
		val resources = mock(Resources::class.java)
		`when`(activity.resources).thenReturn(resources)

		val configuration = mock(Configuration::class.java)
		`when`(resources.configuration)
			.thenReturn(configuration)

		return this
	}

	fun addStringResource(key: String, value: String): MockContext {
		resourceCount++

		`when`(
			activity.resources
				.getIdentifier(
					key,
					"string",
					activity.packageName
				)
		).thenReturn(resourceCount)

		`when`(activity.getString(resourceCount))
			.thenReturn(value)

		return this
	}
}
