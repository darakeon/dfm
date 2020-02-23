package com.darakeon.dfm.auth

import android.support.test.InstrumentationRegistry
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class SPAndroidTest {
	@Test
	fun getAndSetValue() {
		val context = InstrumentationRegistry.getContext()

		context.setValue("Navi", "Hey, listen!")

		assertThat(context.getValue("Navi"), `is`("Hey, listen!"))
	}
}
