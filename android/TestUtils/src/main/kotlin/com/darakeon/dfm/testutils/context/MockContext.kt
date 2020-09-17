package com.darakeon.dfm.testutils.context

import android.annotation.SuppressLint
import android.app.Activity
import android.content.Context
import android.content.SharedPreferences
import android.content.pm.ActivityInfo
import android.content.pm.ApplicationInfo
import android.content.pm.PackageManager
import android.content.pm.ResolveInfo
import android.content.res.Configuration
import android.content.res.Resources
import android.net.ConnectivityManager
import android.net.Network
import android.net.NetworkCapabilities
import android.os.Build
import android.view.LayoutInflater
import androidx.annotation.RequiresApi
import org.mockito.ArgumentMatchers.any
import org.mockito.ArgumentMatchers.anyInt
import org.mockito.ArgumentMatchers.anyString
import org.mockito.Mockito.`when`
import org.mockito.Mockito.mock
import kotlin.reflect.KClass

class MockContext<A: Activity>(type: KClass<A>) {
	val activity: A = mock(type.java)
	private val manager = mock(ConnectivityManager::class.java)

	fun mockInternet(): MockContext<A> {
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

	@RequiresApi(Build.VERSION_CODES.M)
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

	@RequiresApi(Build.VERSION_CODES.M)
	private fun mockEmptyConnectionNew() {
		`when`(manager.activeNetwork).thenReturn(null)
	}

	@Suppress("DEPRECATION")
	private fun mockEmptyConnectionOld() {
		`when`(manager.activeNetworkInfo).thenReturn(null)
	}

	fun mockResources(): MockContext<A> {
		val resources = mock(Resources::class.java)
		`when`(activity.resources).thenReturn(resources)

		val configuration = mock(Configuration::class.java)
		`when`(resources.configuration)
			.thenReturn(configuration)

		return this
	}

	fun addStringResource(key: Int, value: String): MockContext<A> {
		`when`(activity.getString(key))
			.thenReturn(value)

		return this
	}

	@SuppressLint("CommitPrefEdits")
	fun mockSharedPreferences(): MockContext<A> {
		val prefs = mock(SharedPreferences::class.java)
		`when`(activity.getSharedPreferences("DfM", Context.MODE_PRIVATE))
			.thenReturn(prefs)

		val editor = mock(SharedPreferences.Editor::class.java)
		`when`(prefs.edit()).thenReturn(editor)

		val dic = HashMap<String, String>()
		`when`(editor.putString(anyString(), anyString()))
			.then {
				val key = it.arguments[0].toString()
				val value = it.arguments[1].toString()
				dic[key] = value
				null
			}
		`when`(prefs.getString(anyString(), anyString()))
			.then {
				val key = it.arguments[0].toString()
				val defaultValue = it.arguments[1].toString()

				if (dic.containsKey(key))
					dic[key]
				else
					defaultValue
			}

		return this
	}

	fun mockExternalCall(): MockContext<A> {
		val packManager = mock(PackageManager::class.java)
		`when`(activity.packageManager).thenReturn(packManager)

		val info = ResolveInfo()
		info.activityInfo = ActivityInfo()
		info.activityInfo.name = ""
		info.activityInfo.applicationInfo = ApplicationInfo()
		info.activityInfo.applicationInfo.packageName = ""
		`when`(packManager.resolveActivity(any(), anyInt()))
			.thenReturn(info)

		return this
	}

	fun mockInflater(): LayoutInflater {
		val inflater = mock(LayoutInflater::class.java)

		`when`(activity.getSystemService(
			Context.LAYOUT_INFLATER_SERVICE
		)).thenReturn(inflater)

		return inflater
	}
}
