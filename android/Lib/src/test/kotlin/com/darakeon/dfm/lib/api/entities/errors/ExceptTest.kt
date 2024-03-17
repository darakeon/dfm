package com.darakeon.dfm.lib.api.entities.errors

import com.darakeon.dfm.testutils.BaseTest
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class ExceptTest: BaseTest() {
	@Test
	fun mostInner_level1() {
		val level1 = Except(
			"className", "message", null, "stackTrace", "source",
		)

		assertThat(level1.mostInner(), `is`(level1))
	}

	@Test
	fun mostInner_level2() {
		val level1 = Except(
			"className", "message", null, "stackTrace", "source",
		)

		val level2 = Except(
			"className", "message", level1, "stackTrace", "source",
		)

		assertThat(level2.mostInner(), `is`(level1))
	}

	@Test
	fun mostInner_level3() {
		val level1 = Except(
			"className", "message", null, "stackTrace", "source",
		)

		val level2 = Except(
			"className", "message", level1, "stackTrace", "source",
		)

		val level3 = Except(
			"className", "message", level2, "stackTrace", "source",
		)

		assertThat(level3.mostInner(), `is`(level1))
	}

	@Test
	fun mostInner_level4() {
		val level1 = Except(
			"className", "message", null, "stackTrace", "source",
		)

		val level2 = Except(
			"className", "message", level1, "stackTrace", "source",
		)

		val level3 = Except(
			"className", "message", level2, "stackTrace", "source",
		)

		val level4 = Except(
			"className", "message", level3, "stackTrace", "source",
		)

		assertThat(level4.mostInner(), `is`(level1))
	}
}
