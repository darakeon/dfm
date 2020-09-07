package com.darakeon.dfm.lib.api.entities.status

import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class ErrorLogTest {
	private val defaultDate = "19860327012000999999"
	private val defaultException = Exception(
		"className", "message", null, "stackTrace", "source",
	)

	@Test
	fun id() {
		val errorLog = ErrorLog("20200912224537886752", defaultException)
		assertThat(errorLog.id(), `is`(537886752))
	}

	@Test
	fun id_outOfPattern() {
		val errorLog = ErrorLog("something", defaultException)
		assertThat(errorLog.id(), `is`(0))
	}

	@Test
	fun title() {
		val errorLog = ErrorLog("20200912224537886752", defaultException)
		assertThat(errorLog.title(), `is`("2020-09-12 22:45"))
	}

	@Test
	fun title_outOfPattern() {
		val errorLog = ErrorLog("something", defaultException)
		assertThat(errorLog.title(), `is`("something"))
	}

	@Test
	fun message() {
		val exception = Exception(
			"className", "message", null, "stackTrace", "source",
		)

		val errorLog = ErrorLog(defaultDate, exception)
		assertThat(errorLog.message(), `is`("message"))
	}

	@Test
	fun message_inner() {
		val inner = Exception(
			"className", "message inner", null, "stackTrace", "source",
		)

		val outer = Exception(
			"className", "message outer", inner, "stackTrace", "source",
		)

		val errorLog = ErrorLog(defaultDate, outer)
		assertThat(errorLog.message(), `is`("message inner"))
	}

	@Test
	fun message_innerOfInner() {
		val inner = Exception(
			"className", "message inner", null, "stackTrace", "source",
		)

		val middle = Exception(
			"className", "message middle", inner, "stackTrace", "source",
		)

		val outer = Exception(
			"className", "message outer", middle, "stackTrace", "source",
		)

		val errorLog = ErrorLog(defaultDate, outer)
		assertThat(errorLog.message(), `is`("message inner"))
	}
}
