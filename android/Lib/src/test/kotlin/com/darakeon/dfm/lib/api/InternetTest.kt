package com.darakeon.dfm.lib.api

import android.net.NetworkCapabilities
import android.os.Build
import com.darakeon.dfm.lib.utils.mockContext
import com.darakeon.dfm.testutils.BaseTest
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertFalse
import org.junit.Assert.assertTrue
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.annotation.Config

@RunWith(RobolectricTestRunner::class)
class InternetTest: BaseTest() {
	@Test
	@Config(sdk = [Build.VERSION_CODES.M])
	fun isOnline_23AndSoOn() {
		assertThat(Build.VERSION.SDK_INT, `is`(Build.VERSION_CODES.M))

		val mockContext = mockContext().mockInternet()
		val context = mockContext.activity

		val offline = Internet.isOffline(context)

		assertFalse(offline)
	}

	@Test
	@Config(sdk = [Build.VERSION_CODES.LOLLIPOP])
	fun isOnline_below23() {
		assertThat(Build.VERSION.SDK_INT, `is`(Build.VERSION_CODES.LOLLIPOP))

		val mockContext = mockContext().mockInternet()
		val context = mockContext.activity

		val offline = Internet.isOffline(context)

		assertFalse(offline)
	}

	@Test
	@Config(sdk = [Build.VERSION_CODES.M])
	fun isOfflineBecauseOfNotConnected_23AndSoOn() {
		assertThat(Build.VERSION.SDK_INT, `is`(Build.VERSION_CODES.M))

		val mockContext = mockContext().mockInternet()
		mockContext.mockFailConnection()

		val context = mockContext.activity

		val offline = Internet.isOffline(context)

		assertTrue(offline)
	}

	@Test
	@Suppress("DEPRECATION")
	@Config(sdk = [Build.VERSION_CODES.LOLLIPOP])
	fun isOfflineBecauseOfNotConnected_below23() {
		assertThat(Build.VERSION.SDK_INT, `is`(Build.VERSION_CODES.LOLLIPOP))

		val mockContext = mockContext().mockInternet()
		mockContext.mockFailConnection()

		val context = mockContext.activity

		val offline = Internet.isOffline(context)

		assertTrue(offline)
	}

	@Test
	@Config(sdk = [Build.VERSION_CODES.M])
	fun isOfflineBecauseOfNull_23AndSoOn() {
		assertThat(Build.VERSION.SDK_INT, `is`(Build.VERSION_CODES.M))

		val mockContext = mockContext().mockInternet()
		mockContext.mockEmptyConnection()

		val context = mockContext.activity

		val offline = Internet.isOffline(context)

		assertTrue(offline)
	}

	@Test
	@Suppress("DEPRECATION")
	@Config(sdk = [Build.VERSION_CODES.LOLLIPOP])
	fun isOfflineBecauseOfNull_below23() {
		assertThat(Build.VERSION.SDK_INT, `is`(Build.VERSION_CODES.LOLLIPOP))

		val mockContext = mockContext().mockInternet()
		mockContext.mockEmptyConnection()

		val context = mockContext.activity

		val offline = Internet.isOffline(context)

		assertTrue(offline)
	}

	@Test
	@Config(sdk = [Build.VERSION_CODES.M])
	fun isNotEmulator_23AndSoOn() {
		assertThat(Build.VERSION.SDK_INT, `is`(Build.VERSION_CODES.M))

		val mockContext = mockContext().mockInternet(
			NetworkCapabilities.TRANSPORT_WIFI
		)

		val context = mockContext.activity

		val emulator = Internet.isEmulator(context)
		print(emulator)

		assertFalse(emulator)
	}

	@Test
	@Config(sdk = [Build.VERSION_CODES.M])
	fun isEmulator_23AndSoOn() {
		assertThat(Build.VERSION.SDK_INT, `is`(Build.VERSION_CODES.M))

		val mockContext = mockContext().mockInternet(
			NetworkCapabilities.TRANSPORT_ETHERNET
		)

		val context = mockContext.activity

		val emulator = Internet.isEmulator(context)

		assertTrue(emulator)
	}

	@Test
	@Suppress("DEPRECATION")
	@Config(sdk = [Build.VERSION_CODES.LOLLIPOP])
	fun isEmulator_below23() {
		assertThat(Build.VERSION.SDK_INT, `is`(Build.VERSION_CODES.LOLLIPOP))

		val mockContext = mockContext().mockInternet()
		mockContext.mockEmptyConnection()

		val context = mockContext.activity

		val emulator = Internet.isEmulator(context)

		assertFalse(emulator)
	}
}
