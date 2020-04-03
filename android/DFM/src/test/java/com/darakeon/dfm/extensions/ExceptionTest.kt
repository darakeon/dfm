package com.darakeon.dfm.extensions

import com.darakeon.dfm.utils.TestException
import com.darakeon.dfm.utils.log.LogRule
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Rule
import org.junit.Test

class ExceptionTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun getStackTraceText() {
		val ex = TestException("test")
		ex.stackTrace = arrayOf(
			StackTraceElement("class0", "method0", "file0", 1),
			StackTraceElement("class1", "method1", "file1", 2)
		)

		assertThat(ex.stackTraceText, `is`(
			"class0.method0(file0:1)\n"
			+ "class1.method1(file1:2)\n"
		))
	}
}
