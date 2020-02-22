package com.darakeon.dfm.api

import android.support.test.InstrumentationRegistry
import org.junit.Assert.assertFalse
import org.junit.Test

class InternetAndroidTest {
	@Test
	fun isOnline() {
		val context = InstrumentationRegistry.getTargetContext()

		val isOffline = Internet.isOffline(context)

		assertFalse(isOffline)
	}
}
