package com.darakeon.dfm.extensions

import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNull
import org.junit.Test

class GenericTest {
	@Test
	fun getChild() {
		val test = TestParentClass("value")
		val value = test.getChild("parent")
		assertThat(value.toString(), `is`("value"))
	}

	@Test
	fun getChildWithoutProperty() {
		val test = TestParentClass("value")
		val value = test.getChild("not_existent")
		assertNull(value)
	}

	@Test
	fun getGrandChild() {
		val test = TestParentClass("parent", TestChildClass("kid"))
		val value = test.getChild("child", "kid")
		assertThat(value.toString(), `is`("kid"))
	}

	@Test
	fun getGrandChildWithoutProperty() {
		val test = TestParentClass("parent", TestChildClass("kid"))
		val value = test.getChild("not_existent", "not_existent")
		assertNull(value)
	}

	@Suppress("unused")
	class TestParentClass(
		private val parent: String,
		private val child: TestChildClass? = null
	)

	@Suppress("unused")
	class TestChildClass(
		private val kid: String
	)
}
