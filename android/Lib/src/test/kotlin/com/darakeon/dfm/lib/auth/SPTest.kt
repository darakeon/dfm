package com.darakeon.dfm.lib.auth

import com.darakeon.dfm.lib.utils.mockContext
import com.darakeon.dfm.testutils.LogRule
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Rule
import org.junit.Test

class SPTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun getAndSetValue() {
		val mockContext = mockContext().mockSharedPreferences()
		val context = mockContext.activity

		context.setValue("Navi", "Hey, listen!")

		assertThat(context.getValue("Navi"), `is`("Hey, listen!"))
	}
}
