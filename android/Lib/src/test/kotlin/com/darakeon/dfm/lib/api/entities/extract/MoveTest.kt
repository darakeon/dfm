package com.darakeon.dfm.lib.api.entities.extract

import com.darakeon.dfm.lib.api.entities.Date
import com.darakeon.dfm.testutils.BaseTest
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test
import java.util.UUID

class MoveTest: BaseTest() {
	@Test
	fun checkDate() {
		val move = Move("desc", 1986, 3, 27, 5889.93, false, UUID.randomUUID())
		assertThat(move.date, `is`(Date(1986, 3, 27)))
	}
}
