package com.darakeon.dfm.extensions

import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class GenericTest {
	@Test
	fun getChildOrMe() {
		val test = TestClass("value")
		val value = test.getChildOrMe("field").toString()
		assertThat(value, `is`("value"))
	}

	@Test
	fun getChildOrMeWithoutProperty() {
		val test = TestClass("value")
		val value = test.getChildOrMe("not_existent").toString()
		assertThat(value, `is`("no_field"))
	}

	class TestClass(private val field: String) {
		override fun toString(): String {
			return "no_field"
		}
	}
}
