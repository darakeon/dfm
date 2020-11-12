package com.darakeon.dfm.lib.extensions

import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.getDecimal
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class StringExtensionTest: BaseTest() {
	@Test
	fun toDoubleByCulture() {
		val text = "3.27".getDecimal()
		assertThat(text.toDoubleByCulture(), `is`(3.27))
	}
}
