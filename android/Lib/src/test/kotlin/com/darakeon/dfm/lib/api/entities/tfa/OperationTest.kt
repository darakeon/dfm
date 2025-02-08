package com.darakeon.dfm.lib.api.entities.tfa

import com.darakeon.dfm.testutils.BaseTest
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class OperationTest: BaseTest() {
	@Test
	fun validate() {
		assertThat(Operation.get(0), `is`(Operation.Validate))
	}

	@Test
	fun askRemove() {
		assertThat(Operation.get(1), `is`(Operation.AskRemove))
	}
}
