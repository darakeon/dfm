package com.darakeon.dfm.api

import android.os.Build
import com.darakeon.dfm.utils.ConfigChanger.setAndroidVersion
import com.darakeon.dfm.utils.MockContext
import org.junit.Assert.assertFalse
import org.junit.Assert.assertTrue
import org.junit.Test

class InternetTest {
	@Test
	fun isOnline_23AndSoOn() {
		setAndroidVersion(Build.VERSION_CODES.M)

		val mockContext = MockContext().mockInternet()
		val context = mockContext.activity

		val isOffline = Internet.isOffline(context)

		assertFalse(isOffline)
	}

	@Test
	fun isOnline_below23() {
		setAndroidVersion(Build.VERSION_CODES.LOLLIPOP)

		val mockContext = MockContext().mockInternet()
		val context = mockContext.activity

		val isOffline = Internet.isOffline(context)

		assertFalse(isOffline)
	}

	@Test
	fun isOfflineBecauseOfNotConnected_23AndSoOn() {
		setAndroidVersion(Build.VERSION_CODES.M)

		val mockContext = MockContext().mockInternet()
		mockContext.mockFailConnection()

		val context = mockContext.activity

		val isOffline = Internet.isOffline(context)

		assertTrue(isOffline)
	}

	@Test
	@Suppress("DEPRECATION")
	fun isOfflineBecauseOfNotConnected_below23() {
		setAndroidVersion(Build.VERSION_CODES.LOLLIPOP)

		val mockContext = MockContext().mockInternet()
		mockContext.mockFailConnection()

		val context = mockContext.activity

		val isOffline = Internet.isOffline(context)

		assertTrue(isOffline)
	}

	@Test
	fun isOfflineBecauseOfNull_23AndSoOn() {
		setAndroidVersion(Build.VERSION_CODES.M)

		val mockContext = MockContext().mockInternet()
		mockContext.mockEmptyConnection()

		val context = mockContext.activity

		val isOffline = Internet.isOffline(context)

		assertTrue(isOffline)
	}

	@Test
	@Suppress("DEPRECATION")
	fun isOfflineBecauseOfNull_below23() {
		setAndroidVersion(Build.VERSION_CODES.LOLLIPOP)

		val mockContext = MockContext().mockInternet()
		mockContext.mockEmptyConnection()

		val context = mockContext.activity

		val isOffline = Internet.isOffline(context)

		assertTrue(isOffline)
	}
}
