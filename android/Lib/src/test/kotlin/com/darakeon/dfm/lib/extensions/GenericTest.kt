package com.darakeon.dfm.lib.extensions

import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.setPrivate
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNull
import org.junit.Test

class GenericTest: BaseTest() {
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

	class TestParentClass(
		private val child: TestChildClass
	) {
		val pubChild: TestChildClass
			get() = child
	}

	class TestChildClass(
		private var pet: Int
	) {
		val pubPet: Int
			get() = pet
	}
}
