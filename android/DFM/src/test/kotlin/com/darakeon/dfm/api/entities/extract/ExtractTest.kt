package com.darakeon.dfm.api.entities.extract

import com.darakeon.dfm.utils.log.LogRule
import org.junit.Assert.assertEquals
import org.junit.Assert.assertNotEquals
import org.junit.Rule
import org.junit.Test
import java.util.UUID

class ExtractTest {
	@get:Rule
	val log = LogRule()

	private val fakeGuid
		get() = UUID.randomUUID()

	@Test
	fun equals() {
		val move1 = Move("move_1", 2020, 2, 22, 1.0, true, fakeGuid)
		val move2 = Move("move_2", 2020, 2, 22, 2.0, true, fakeGuid)
		val move3 = Move("move_3", 2020, 2, 22, 3.0, true, fakeGuid)

		val firstEntity = Extract(
			"extract", 6.0, false, listOf(move1, move2, move3)
		)

		val secondEntity = Extract(
			"extract", 6.0, false, listOf(move1, move2, move3)
		)

		assertEquals(firstEntity, secondEntity)
	}

	@Test
	fun notEqualsBecauseOfList() {
		val move1 = Move("move_1", 2020, 2, 22, 1.0, true, fakeGuid)
		val move2 = Move("move_2", 2020, 2, 22, 2.0, true, fakeGuid)
		val move3 = Move("move_3", 2020, 2, 22, 3.0, true, fakeGuid)

		val firstEntity = Extract(
			"extract", 6.0, false, listOf(move1, move2)
		)

		val secondEntity = Extract(
			"extract", 6.0, false, listOf(move1, move3)
		)

		assertNotEquals(firstEntity, secondEntity)
	}

	@Test
	fun notEqualsBecauseOfTitle() {
		val move1 = Move("move_1", 2020, 2, 22, 1.0, true, fakeGuid)
		val move2 = Move("move_2", 2020, 2, 22, 2.0, true, fakeGuid)
		val move3 = Move("move_3", 2020, 2, 22, 3.0, true, fakeGuid)

		val firstEntity = Extract(
			"extract 1", 6.0, false, listOf(move1, move2, move3)
		)

		val secondEntity = Extract(
			"extract 2", 6.0, false, listOf(move1, move2, move3)
		)

		assertNotEquals(firstEntity, secondEntity)
	}

	@Test
	fun notEqualsBecauseOfTotal() {
		val move1 = Move("move_1", 2020, 2, 22, 1.0, true, fakeGuid)
		val move2 = Move("move_2", 2020, 2, 22, 2.0, true, fakeGuid)
		val move3 = Move("move_3", 2020, 2, 22, 3.0, true, fakeGuid)

		val firstEntity = Extract(
			"extract", 6.0, false, listOf(move1, move2, move3)
		)

		val secondEntity = Extract(
			"extract", 7.0, false, listOf(move1, move2, move3)
		)

		assertNotEquals(firstEntity, secondEntity)
	}

	@Test
	fun notEqualsBecauseOfCanCheck() {
		val move1 = Move("move_1", 2020, 2, 22, 1.0, true, fakeGuid)
		val move2 = Move("move_2", 2020, 2, 22, 2.0, true, fakeGuid)
		val move3 = Move("move_3", 2020, 2, 22, 3.0, true, fakeGuid)

		val firstEntity = Extract(
			"extract", 6.0, false, listOf(move1, move2, move3)
		)

		val secondEntity = Extract(
			"extract", 6.0, true, listOf(move1, move2, move3)
		)

		assertNotEquals(firstEntity, secondEntity)
	}
}
