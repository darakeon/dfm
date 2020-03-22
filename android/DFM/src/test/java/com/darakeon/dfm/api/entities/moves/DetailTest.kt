package com.darakeon.dfm.api.entities.moves

import org.junit.Assert.assertEquals
import org.junit.Assert.assertNotEquals
import org.junit.Test

class DetailTest {
	@Test
	fun equals() {
		val detail1 = Detail("desc", 1, 1.0)
		val detail2 = Detail("desc", 1, 1.0)

		assertEquals(detail1, detail2)
	}

	@Test
	fun notEqualsBecauseOfDescription() {
		val detail1 = Detail("desc", 1, 1.0)
		val detail2 = Detail("desc_diff", 1, 1.0)

		assertNotEquals(detail1, detail2)
	}

	@Test
	fun notEqualsBecauseOfAmount() {
		val detail1 = Detail("desc", 1, 1.0)
		val detail2 = Detail("desc", 2, 1.0)

		assertNotEquals(detail1, detail2)
	}

	@Test
	fun notEqualsBecauseOfValue() {
		val detail1 = Detail("desc", 1, 1.0)
		val detail2 = Detail("desc", 1, 2.0)

		assertNotEquals(detail1, detail2)
	}
}
