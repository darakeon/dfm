package com.darakeon.dfm.api.entities.summary

import com.darakeon.dfm.utils.log.LogRule
import org.junit.Assert.assertEquals
import org.junit.Assert.assertNotEquals
import org.junit.Rule
import org.junit.Test

class SummaryTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun equals() {
		val month1 = Month("Month_1", 1, 1.0)
		val month2 = Month("Month_2", 2, 2.0)
		val month3 = Month("Month_3", 3, 3.0)

		val firstEntity = Summary(
			"Summary", 6.0, listOf(month1, month2, month3)
		)

		val secondEntity = Summary(
			"Summary", 6.0, listOf(month1, month2, month3)
		)

		assertEquals(firstEntity, secondEntity)
	}

	@Test
	fun notEqualsBecauseOfList() {
		val month1 = Month("Month_1", 1, 1.0)
		val month2 = Month("Month_2", 2, 2.0)
		val month3 = Month("Month_3", 3, 3.0)

		val firstEntity = Summary(
			"Summary", 6.0, listOf(month1, month2)
		)

		val secondEntity = Summary(
			"Summary", 6.0, listOf(month1, month3)
		)

		assertNotEquals(firstEntity, secondEntity)
	}

	@Test
	fun notEqualsBecauseOfTitle() {
		val month1 = Month("Month_1", 1, 1.0)
		val month2 = Month("Month_2", 2, 2.0)
		val month3 = Month("Month_3", 3, 3.0)

		val firstEntity = Summary(
			"Summary 1", 6.0, listOf(month1, month2, month3)
		)

		val secondEntity = Summary(
			"Summary 2", 6.0, listOf(month1, month2, month3)
		)

		assertNotEquals(firstEntity, secondEntity)
	}

	@Test
	fun notEqualsBecauseOfTotal() {
		val month1 = Month("Month_1", 1, 1.0)
		val month2 = Month("Month_2", 2, 2.0)
		val month3 = Month("Month_3", 3, 3.0)

		val firstEntity = Summary(
			"Summary", 6.0, listOf(month1, month2, month3)
		)

		val secondEntity = Summary(
			"Summary", 7.0, listOf(month1, month2, month3)
		)

		assertNotEquals(firstEntity, secondEntity)
	}
}
