package com.darakeon.dfm.auth

import com.darakeon.dfm.utils.MockContext
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class SPTest {
	@Test
	fun getAndSetValue() {
		val mockContext = MockContext().mockSharedPreferences()
		val context = mockContext.activity

		context.setValue("Navi", "Hey, listen!")

		assertThat(context.getValue("Navi"), `is`("Hey, listen!"))
	}
}
