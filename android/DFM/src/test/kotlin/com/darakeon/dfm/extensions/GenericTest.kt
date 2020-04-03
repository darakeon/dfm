package com.darakeon.dfm.extensions

import com.darakeon.dfm.utils.log.LogRule
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNull
import org.junit.Rule
import org.junit.Test

class GenericTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun getChild() {
		val test = TestChildClass(27)
		val value = test.getPrivate<Int>("pet")
		assertThat(value, `is`(27))
	}

	@Test
	fun getChildWithoutProperty() {
		val test = TestChildClass(27)
		val value = test.getPrivate<Any>("not_existent")
		assertNull(value)
	}

	@Test
	fun getGrandChild() {
		val test = TestParentClass(TestChildClass(27))
		val value = test.getPrivate<Int>("child", "pet")
		assertThat(value, `is`(27))
	}

	@Test
	fun getGrandChildWithoutProperty() {
		val test = TestParentClass(TestChildClass(27))
		val value = test.getPrivate<Any>("not_existent", "not_existent")
		assertNull(value)
	}

	@Test
	fun setChild() {
		val test = TestChildClass(13)
		test.setPrivate("pet"){27}
		assertThat(test.pubPet, `is`(27))
	}

	@Test
	fun setGrandChild() {
		val test = TestParentClass(TestChildClass(13))
		test.setPrivate("child", "pet"){27}
		assertThat(test.pubChild.pubPet, `is`(27))
	}

	@Suppress("unused")
	class TestParentClass(
		private val child: TestChildClass
	) {
		val pubChild: TestChildClass
			get() = child
	}

	@Suppress("unused")
	class TestChildClass(
		private var pet: Int
	) {
		val pubPet: Int
			get() = pet
	}
}
