package com.darakeon.dfm.api.entities.moves

import com.darakeon.dfm.api.entities.ComboItem
import org.junit.Assert.assertEquals
import org.junit.Assert.assertNotEquals
import org.junit.Test

class MoveCreationTest {
	@Test
	fun equals() {
		val move = Move()
		val categories = arrayOf(ComboItem("c", "c"))
		val natures = arrayOf(ComboItem("n", "n"))
		val accounts = arrayOf(ComboItem("a", "a"))

		val creation1 = MoveCreation(move, true, categories, natures, accounts)
		val creation2 = MoveCreation(move, true, categories, natures, accounts)

		assertEquals(creation1, creation2)
	}

	@Test
	fun notEqualsBecauseOfMove() {
		val move1 = Move()
		move1.description = "1"
		val move2 = Move()
		move2.description = "2"

		val categories = arrayOf(ComboItem("c", "c"))
		val natures = arrayOf(ComboItem("n", "n"))
		val accounts = arrayOf(ComboItem("a", "a"))

		val creation1 = MoveCreation(move1, true, categories, natures, accounts)
		val creation2 = MoveCreation(move2, true, categories, natures, accounts)

		assertNotEquals(creation1, creation2)
	}

	@Test
	fun notEqualsBecauseOfCategories() {
		val move = Move()

		val categories1 = arrayOf(ComboItem("c1", "c1"))
		val categories2 = arrayOf(ComboItem("c2", "c2"))

		val natures = arrayOf(ComboItem("n", "n"))
		val accounts = arrayOf(ComboItem("a", "a"))

		val creation1 = MoveCreation(move, true, categories1, natures, accounts)
		val creation2 = MoveCreation(move, true, categories2, natures, accounts)

		assertNotEquals(creation1, creation2)
	}

	@Test
	fun notEqualsBecauseOfNatures() {
		val move = Move()
		val categories = arrayOf(ComboItem("c", "c"))

		val natures1 = arrayOf(ComboItem("n1", "n1"))
		val natures2 = arrayOf(ComboItem("n2", "n2"))

		val accounts = arrayOf(ComboItem("a", "a"))

		val creation1 = MoveCreation(move, true, categories, natures1, accounts)
		val creation2 = MoveCreation(move, true, categories, natures2, accounts)

		assertNotEquals(creation1, creation2)
	}

	@Test
	fun notEqualsBecauseOfAccounts() {
		val move = Move()
		val categories = arrayOf(ComboItem("c", "c"))
		val natures = arrayOf(ComboItem("n", "n"))

		val accounts1 = arrayOf(ComboItem("a1", "a1"))
		val accounts2 = arrayOf(ComboItem("a2", "a2"))

		val creation1 = MoveCreation(move, true, categories, natures, accounts1)
		val creation2 = MoveCreation(move, true, categories, natures, accounts2)

		assertNotEquals(creation1, creation2)
	}
}
