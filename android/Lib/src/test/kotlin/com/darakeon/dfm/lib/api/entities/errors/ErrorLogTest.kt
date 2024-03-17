package com.darakeon.dfm.lib.api.entities.errors

import com.darakeon.dfm.testutils.BaseTest
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class ErrorLogTest: BaseTest() {
	private val defaultDate = "19860327012000999999"
	private val defaultException = Except(
		"className", "message", null, "stackTrace", "source",
	)

	@Test
	fun date() {
		val errorLog = ErrorLog(
			"20200912224537886752", false, defaultException, 1, 0
		)
		assertThat(errorLog.date(), `is`("2020-09-12 22:45"))
	}

	@Test
	fun date_outOfPattern() {
		val errorLog = ErrorLog(
			"something", false, defaultException, 1, 0
		)
		assertThat(errorLog.date(), `is`("something"))
	}

	@Test
	fun message() {
		val exception = Except(
			"className", "message", null, "stackTrace", "source",
		)

		val errorLog = ErrorLog(
			defaultDate, false, exception, 1, 0
		)

		assertThat(errorLog.message(), `is`("message"))
	}

	@Test
	fun message_inner() {
		val inner = Except(
			"className", "message inner", null, "stackTrace", "source",
		)

		val outer = Except(
			"className", "message outer", inner, "stackTrace", "source",
		)

		val errorLog = ErrorLog(
			defaultDate, false, outer, 1, 0
		)

		assertThat(errorLog.message(), `is`("message inner"))
	}

	@Test
	fun message_innerOfInner() {
		val inner = Except(
			"className", "message inner", null, "stackTrace", "source",
		)

		val middle = Except(
			"className", "message middle", inner, "stackTrace", "source",
		)

		val outer = Except(
			"className", "message outer", middle, "stackTrace", "source",
		)

		val errorLog = ErrorLog(
			defaultDate, false, outer, 1, 0
		)

		assertThat(errorLog.message(), `is`("message inner"))
	}
}
