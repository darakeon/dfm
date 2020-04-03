package com.darakeon.dfm.auth

import com.darakeon.dfm.utils.activity.MockContext
import com.darakeon.dfm.utils.log.LogRule
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Rule
import org.junit.Test

class SPTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun getAndSetValue() {
		val mockContext = MockContext().mockSharedPreferences()
		val context = mockContext.activity

		context.setValue("Navi", "Hey, listen!")

		assertThat(context.getValue("Navi"), `is`("Hey, listen!"))
	}
}
