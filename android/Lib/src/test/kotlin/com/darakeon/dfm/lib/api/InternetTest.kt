package com.darakeon.dfm.lib.api

import com.darakeon.dfm.lib.utils.mockContext
import com.darakeon.dfm.testutils.BaseTest
import org.junit.Assert.assertFalse
import org.junit.Assert.assertTrue
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class InternetTest: BaseTest() {
	@Test
	fun isOnline() {
		val mockContext = mockContext().mockInternet()
		val context = mockContext.activity

		val offline = Internet(context).isOffline()

		assertFalse(offline)
	}

	@Test
	fun isOfflineBecauseOfNotConnected() {
		val mockContext = mockContext().mockInternet()
		mockContext.mockFailConnection()

		val context = mockContext.activity

		val offline = Internet(context).isOffline()

		assertTrue(offline)
	}


	@Test
	fun isOfflineBecauseOfNull() {
		val mockContext = mockContext().mockInternet()
		mockContext.mockEmptyConnection()

		val context = mockContext.activity

		val offline = Internet(context).isOffline()

		assertTrue(offline)
	}

	@Test
	fun isNotEmulator() {
		val emulator = Internet.isEmulator()
		assertFalse(emulator)
	}
}
