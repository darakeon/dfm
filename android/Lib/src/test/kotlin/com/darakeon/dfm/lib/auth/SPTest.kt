package com.darakeon.dfm.lib.auth

import com.darakeon.dfm.lib.utils.mockContext
import com.darakeon.dfm.testutils.BaseTest
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class SPTest: BaseTest() {
	@Test
	fun getAndSetValue() {
		val mockContext = mockContext().mockSharedPreferences()
		val context = mockContext.activity

		context.setValue("Navi", "Hey, listen!")

		assertThat(context.getValue("Navi"), `is`("Hey, listen!"))
	}
}
