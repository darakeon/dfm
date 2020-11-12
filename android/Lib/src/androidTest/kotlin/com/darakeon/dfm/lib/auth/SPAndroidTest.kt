package com.darakeon.dfm.lib.auth

import android.support.test.InstrumentationRegistry
import com.darakeon.dfm.testutils.BaseTest
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class SPAndroidTest: BaseTest() {
	@Test
	fun getAndSetValue() {
		val context = InstrumentationRegistry.getContext()

		context.setValue("Navi", "Hey, listen!")

		assertThat(context.getValue("Navi"), `is`("Hey, listen!"))
	}
}
