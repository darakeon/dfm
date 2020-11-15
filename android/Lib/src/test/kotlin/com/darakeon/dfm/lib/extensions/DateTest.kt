package com.darakeon.dfm.lib.extensions

import com.darakeon.dfm.testutils.BaseTest
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test
import java.util.Calendar
import java.util.Locale

class DateTest: BaseTest() {
	@Test
	fun formatCalendar() {
		val calendar = Calendar.getInstance()
		calendar.set(2020, Calendar.FEBRUARY, 25)
		assertThat(calendar.format(), `is`("2020-02-25"))
	}

	@Test
	fun formatDate() {
		val calendar = Calendar.getInstance()
		calendar.set(2020, Calendar.FEBRUARY, 25)
		assertThat(calendar.time.format(), `is`("2020-02-25"))
	}

	@Test
	fun formatNoDayCalendarPtBr() {
		Locale.setDefault(Locale("pt", "BR"))
		val calendar = Calendar.getInstance()
		calendar.set(2020, Calendar.FEBRUARY, 25)
		assertThat(calendar.formatNoDay(), `is`("fev/2020"))
	}

	@Test
	fun formatNoDayCalendarEnUs() {
		Locale.setDefault(Locale("en", "US"))
		val calendar = Calendar.getInstance()
		calendar.set(2020, Calendar.FEBRUARY, 25)
		assertThat(calendar.formatNoDay(), `is`("Feb/2020"))
	}

	@Test
	fun formatNoDayDatePtBr() {
		Locale.setDefault(Locale("pt", "BR"))
		val calendar = Calendar.getInstance()
		calendar.set(2020, Calendar.FEBRUARY, 25)
		assertThat(calendar.time.formatNoDay(), `is`("fev/2020"))
	}

	@Test
	fun formatNoDayDateEnUs() {
		Locale.setDefault(Locale("en", "US"))
		val calendar = Calendar.getInstance()
		calendar.set(2020, Calendar.FEBRUARY, 25)
		assertThat(calendar.time.formatNoDay(), `is`("Feb/2020"))
	}
}
