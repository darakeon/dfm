package com.darakeon.dfm.lib.api.entities.moves

import com.darakeon.dfm.testutils.BaseTest
import org.junit.Assert.assertEquals
import org.junit.Assert.assertNotEquals
import org.junit.Test

class MoveCreationTest: BaseTest() {
	@Test
	fun equals() {
		val move = Move()

		val creation1 = MoveCreation(move)
		val creation2 = MoveCreation(move)

		assertEquals(creation1, creation2)
	}

	@Test
	fun notEqualsBecauseOfMove() {
		val move1 = Move()
		move1.description = "1"
		val move2 = Move()
		move2.description = "2"

		val creation1 = MoveCreation(move1)
		val creation2 = MoveCreation(move2)

		assertNotEquals(creation1, creation2)
	}
}
