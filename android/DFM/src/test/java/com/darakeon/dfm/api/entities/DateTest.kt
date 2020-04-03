package com.darakeon.dfm.api.entities

import com.darakeon.dfm.utils.log.LogRule
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Rule
import org.junit.Test
import java.util.Calendar

class DateTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun formatInitWithTimes() {
		val date = Date(2019, 5, 4)
		assertThat(date.format(), `is`("2019-05-04"))
	}

	@Test
	fun formatInitWithCalendar() {
		val calendar = Calendar.getInstance()
		calendar.set(Calendar.YEAR, 2019)
		calendar.set(Calendar.MONTH, Calendar.MAY)
		calendar.set(Calendar.DAY_OF_MONTH, 4)

		val date = Date(calendar)
		assertThat(date.format(), `is`("2019-05-04"))
	}
}
