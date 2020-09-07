package com.darakeon.dfm.lib.extensions

import com.darakeon.dfm.testutils.LogRule
import com.darakeon.dfm.testutils.getDecimal
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Rule
import org.junit.Test

class StringExtensionTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun toDoubleByCulture() {
		val text = "3.27".getDecimal()
		assertThat(text.toDoubleByCulture(), `is`(3.27))
	}
}
