package com.darakeon.dfm.extensions

import com.darakeon.dfm.utils.getDecimal
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test
import java.text.DecimalFormat
import java.util.Locale

class StringExtensionTest {
	@Test
	fun toDoubleByCulture() {
		val text = "3.27".getDecimal()
		assertThat(text.toDoubleByCulture(), `is`(3.27))
	}
}
