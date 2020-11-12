package com.darakeon.dfm.extensions

import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.TestException
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class ExceptionTest: BaseTest() {
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
