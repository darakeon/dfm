package com.darakeon.dfm.extensions

import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test
import java.text.DecimalFormat
import java.util.Locale

class StringExtensionTest {
	@Test
	fun toDoubleByCulture() {
		val separator = Locale.getDefault()
		val formatter = DecimalFormat.getInstance(separator) as DecimalFormat
		val decimal = formatter.decimalFormatSymbols.decimalSeparator.toString()

		val text = "3.27".replace(".", decimal)

		assertThat(text.toDoubleByCulture(), `is`(3.27))
	}
}
