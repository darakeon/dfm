package com.darakeon.dfm.api.entities.extract

import com.darakeon.dfm.api.entities.Date
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class MoveTest {
	@Test
	fun checkDate() {
		val move = Move("desc", 1986, 3, 27, 5889.93, false, 1)
		assertThat(move.date, `is`(Date(1986, 3, 27)))
	}
}
