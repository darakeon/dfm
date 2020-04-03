package com.darakeon.dfm.api.entities.moves

import com.darakeon.dfm.api.entities.ComboItem
import com.darakeon.dfm.utils.log.LogRule
import org.junit.Assert.assertEquals
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotEquals
import org.junit.Assert.assertTrue
import org.junit.Rule
import org.junit.Test

class MoveCreationTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun equals() {
		val move = Move()
		val categories = arrayOf(ComboItem("c", "c"))
		val natures = arrayOf(ComboItem("n", "n"))
		val accounts = arrayOf(ComboItem("a", "a"))

		val creation1 = MoveCreation(true, categories, natures, accounts, move)
		val creation2 = MoveCreation(true, categories, natures, accounts, move)

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

		val creation1 = MoveCreation(true, categories, natures, accounts, move1)
		val creation2 = MoveCreation(true, categories, natures, accounts, move2)

		assertNotEquals(creation1, creation2)
	}

	@Test
	fun notEqualsBecauseOfCategories() {
		val move = Move()

		val categories1 = arrayOf(ComboItem("c1", "c1"))
		val categories2 = arrayOf(ComboItem("c2", "c2"))

		val natures = arrayOf(ComboItem("n", "n"))
		val accounts = arrayOf(ComboItem("a", "a"))

		val creation1 = MoveCreation(true, categories1, natures, accounts, move)
		val creation2 = MoveCreation(true, categories2, natures, accounts, move)

		assertNotEquals(creation1, creation2)
	}

	@Test
	fun notEqualsBecauseOfNatures() {
		val move = Move()
		val categories = arrayOf(ComboItem("c", "c"))

		val natures1 = arrayOf(ComboItem("n1", "n1"))
		val natures2 = arrayOf(ComboItem("n2", "n2"))

		val accounts = arrayOf(ComboItem("a", "a"))

		val creation1 = MoveCreation(true, categories, natures1, accounts, move)
		val creation2 = MoveCreation(true, categories, natures2, accounts, move)

		assertNotEquals(creation1, creation2)
	}

	@Test
	fun notEqualsBecauseOfAccounts() {
		val move = Move()
		val categories = arrayOf(ComboItem("c", "c"))
		val natures = arrayOf(ComboItem("n", "n"))

		val accounts1 = arrayOf(ComboItem("a1", "a1"))
		val accounts2 = arrayOf(ComboItem("a2", "a2"))

		val creation1 = MoveCreation(true, categories, natures, accounts1, move)
		val creation2 = MoveCreation(true, categories, natures, accounts2, move)

		assertNotEquals(creation1, creation2)
	}

	@Test
	fun blockedByAccounts() {
		val categories = arrayOf(ComboItem("c", "c"))
		val natures = arrayOf(ComboItem("n", "n"))
		val accounts = emptyArray<ComboItem>()

		val creation = MoveCreation(true, categories, natures, accounts)

		assertTrue(creation.blockedByAccounts())
	}

	@Test
	fun notBlockedByAccounts() {
		val categories = arrayOf(ComboItem("c", "c"))
		val natures = arrayOf(ComboItem("n", "n"))
		val accounts = arrayOf(ComboItem("a", "a"))

		val creation = MoveCreation(true, categories, natures, accounts)

		assertFalse(creation.blockedByAccounts())
	}

	@Test
	fun blockedByCategories() {
		val categories = emptyArray<ComboItem>()
		val natures = arrayOf(ComboItem("n", "n"))
		val accounts = arrayOf(ComboItem("a", "a"))

		val creation = MoveCreation(true, categories, natures, accounts)

		assertTrue(creation.blockedByCategories())
	}

	@Test
	fun notBlockedByCategories() {
		val categories = arrayOf(ComboItem("c", "c"))
		val natures = arrayOf(ComboItem("n", "n"))
		val accounts = arrayOf(ComboItem("a", "a"))

		val creation = MoveCreation(true, categories, natures, accounts)

		assertFalse(creation.blockedByCategories())
	}

	@Test
	fun notBlockedBecauseNotUsingCategories() {
		val categories = emptyArray<ComboItem>()
		val natures = arrayOf(ComboItem("n", "n"))
		val accounts = arrayOf(ComboItem("a", "a"))

		val creation = MoveCreation(false, categories, natures, accounts)

		assertFalse(creation.blockedByCategories())
	}
}
