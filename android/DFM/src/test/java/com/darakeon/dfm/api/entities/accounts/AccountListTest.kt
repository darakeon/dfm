package com.darakeon.dfm.api.entities.accounts

import org.junit.Assert.assertEquals
import org.junit.Assert.assertNotEquals
import org.junit.Test

class AccountListTest {
	@Test
	fun equals() {
		val account0 = Account("Zero", 0.0, "zero")
		val account1 = Account("One", 1.0, "one")
		val account2 = Account("Two", 2.0, "two")

		val firstEntity = AccountList(
			listOf(account0, account1, account2)
		)

		val secondEntity = AccountList(
			listOf(account0, account1, account2)
		)

		assertEquals(firstEntity, secondEntity)
	}

	@Test
	fun notEquals() {
		val account0 = Account("Zero", 0.0, "zero")
		val account1 = Account("One", 1.0, "one")
		val account2 = Account("Two", 2.0, "two")

		val firstEntityList = AccountList(
			listOf(account0, account1)
		)

		val secondEntityList = AccountList(
			listOf(account0, account2)
		)

		assertNotEquals(firstEntityList, secondEntityList)
	}
}
