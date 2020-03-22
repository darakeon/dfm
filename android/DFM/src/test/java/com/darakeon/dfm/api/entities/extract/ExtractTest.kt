package com.darakeon.dfm.api.entities.extract

import org.junit.Assert.assertEquals
import org.junit.Assert.assertNotEquals
import org.junit.Test

class ExtractTest {
	@Test
	fun equals() {
		val move1 = Move("move_1", 2020, 2, 22, 1.0, true, 1)
		val move2 = Move("move_2", 2020, 2, 22, 2.0, true, 2)
		val move3 = Move("move_3", 2020, 2, 22, 3.0, true, 3)

		val firstEntity = Extract(
			arrayOf(move1, move2, move3), "extract", 6.0, false
		)

		val secondEntity = Extract(
			arrayOf(move1, move2, move3), "extract", 6.0, false
		)

		assertEquals(firstEntity, secondEntity)
	}

	@Test
	fun notEqualsBecauseOfList() {
		val move1 = Move("move_1", 2020, 2, 22, 1.0, true, 1)
		val move2 = Move("move_2", 2020, 2, 22, 2.0, true, 2)
		val move3 = Move("move_3", 2020, 2, 22, 3.0, true, 3)

		val firstEntity = Extract(
			arrayOf(move1, move2), "extract", 6.0, false
		)

		val secondEntity = Extract(
			arrayOf(move1, move3), "extract", 6.0, false
		)

		assertNotEquals(firstEntity, secondEntity)
	}

	@Test
	fun notEqualsBecauseOfTitle() {
		val move1 = Move("move_1", 2020, 2, 22, 1.0, true, 1)
		val move2 = Move("move_2", 2020, 2, 22, 2.0, true, 2)
		val move3 = Move("move_3", 2020, 2, 22, 3.0, true, 3)

		val firstEntity = Extract(
			arrayOf(move1, move2, move3), "extract 1", 6.0, false
		)

		val secondEntity = Extract(
			arrayOf(move1, move2, move3), "extract 2", 6.0, false
		)

		assertNotEquals(firstEntity, secondEntity)
	}

	@Test
	fun notEqualsBecauseOfTotal() {
		val move1 = Move("move_1", 2020, 2, 22, 1.0, true, 1)
		val move2 = Move("move_2", 2020, 2, 22, 2.0, true, 2)
		val move3 = Move("move_3", 2020, 2, 22, 3.0, true, 3)

		val firstEntity = Extract(
			arrayOf(move1, move2, move3), "extract", 6.0, false
		)

		val secondEntity = Extract(
			arrayOf(move1, move2, move3), "extract", 7.0, false
		)

		assertNotEquals(firstEntity, secondEntity)
	}

	@Test
	fun notEqualsBecauseOfCanCheck() {
		val move1 = Move("move_1", 2020, 2, 22, 1.0, true, 1)
		val move2 = Move("move_2", 2020, 2, 22, 2.0, true, 2)
		val move3 = Move("move_3", 2020, 2, 22, 3.0, true, 3)

		val firstEntity = Extract(
			arrayOf(move1, move2, move3), "extract", 6.0, false
		)

		val secondEntity = Extract(
			arrayOf(move1, move2, move3), "extract", 6.0, true
		)

		assertNotEquals(firstEntity, secondEntity)
	}
}
