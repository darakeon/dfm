package com.darakeon.dfm.lib.api.entities.moves

import com.darakeon.dfm.lib.api.entities.Date
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.getDecimal
import org.hamcrest.CoreMatchers.hasItem
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.not
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Test
import java.util.UUID

class MoveTest: BaseTest() {
	@Test
	fun natureEnum() {
		val move = Move()

		move.nature = NatureEnum(0)
		assertThat(move.natureEnum, `is`(Nature.Out))

		move.nature = NatureEnum(1)
		assertThat(move.natureEnum, `is`(Nature.In))

		move.nature = NatureEnum(2)
		assertThat(move.natureEnum, `is`(Nature.Transfer))

		move.natureEnum = Nature.Out
		assertThat(move.nature?.code, `is`(0))

		move.natureEnum = Nature.In
		assertThat(move.nature?.code, `is`(1))

		move.natureEnum = Nature.Transfer
		assertThat(move.nature?.code, `is`(2))
	}

	@Test
	fun date() {
		val move = Move()

		move.year = 2020
		move.month = 2
		move.day = 22
		assertThat(move.date, `is`(Date(2020, 2, 22)))

		move.date = Date(2019, 5, 4)
		assertThat(move.year, `is`(2019))
		assertThat(move.month, `is`(5))
		assertThat(move.day, `is`(4))
	}

	@Test
	fun add() {
		val move = Move()

		move.add("desc", 1, 1.0)

		assertThat(
			move.detailList,
			hasItem(Detail("desc", 1, 1.0))
		)
	}

	@Test
	fun remove() {
		val move = Move()

		move.add("desc", 1, 1.0)
		move.remove("desc", 1, 1.0)

		assertThat(
			move.detailList,
			not(hasItem(Detail("desc", 1, 1.0)))
		)
	}

	@Test
	fun setValue() {
		val move = Move()

		val value = "3.27".getDecimal()
		move.setValue(value)

		assertThat(move.value, `is`(3.27))
	}

	@Test
	fun setDefaultDataToCreate() {
		val move = Move()

		move.setDefaultData("url", false)

		assertThat(move.outUrl, `is`("url"))
		assertNull(move.inUrl)

		assertFalse(move.warnCategory)
	}

	@Test
	fun setDefaultDataToEditWithCategory() {
		val move = Move()
		move.guid = UUID.randomUUID()
		move.outUrl = "out"
		move.inUrl = "in"
		move.categoryName = "category"

		move.setDefaultData("url", true)

		assertThat(move.outUrl, `is`("out"))
		assertThat(move.inUrl, `is`("in"))

		assertFalse(move.warnCategory)
	}

	@Test
	fun setDefaultDataToEditWithoutCategory() {
		val move = Move()
		move.guid = UUID.randomUUID()
		move.outUrl = "out"
		move.inUrl = "in"

		move.setDefaultData("url", false)

		assertThat(move.outUrl, `is`("out"))
		assertThat(move.inUrl, `is`("in"))

		assertFalse(move.warnCategory)
	}

	@Test
	fun setDefaultDataToEditWithCategoryWarning() {
		val move = Move()
		move.guid = UUID.randomUUID()
		move.outUrl = "out"
		move.inUrl = "in"
		move.categoryName = "category"

		move.setDefaultData("url", false)

		assertThat(move.outUrl, `is`("out"))
		assertThat(move.inUrl, `is`("in"))

		assertTrue(move.warnCategory)
	}

	@Test
	fun clearNotUsedValuesWithoutDetails() {
		val move = Move()
		move.value = 1.0
		move.detailList = listOf(
			Detail("d", 1, 1.0)
		).toMutableList()

		move.isDetailed = false
		move.clearNotUsedValues()

		assertNotNull(move.value)
		assertThat(move.detailList.size, `is`(0))
	}

	@Test
	fun clearNotUsedValuesWithDetails() {
		val move = Move()
		move.value = 1.0
		move.detailList = listOf(
			Detail("d", 1, 1.0)
		).toMutableList()

		move.isDetailed = true
		move.clearNotUsedValues()

		assertNull(move.value)
		assertThat(move.detailList.size, not(`is`(0)))
	}
}
