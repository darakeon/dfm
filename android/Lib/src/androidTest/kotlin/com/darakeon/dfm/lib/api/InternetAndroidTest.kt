package com.darakeon.dfm.lib.api

import android.support.test.InstrumentationRegistry
import com.darakeon.dfm.testutils.BaseTest
import org.junit.Assert.assertFalse
import org.junit.Test

class InternetAndroidTest: BaseTest() {
	@Test
	fun isOnline() {
		val context = InstrumentationRegistry.getTargetContext()

		val isOffline = Internet.isOffline(context)

		assertFalse(isOffline)
	}
}
