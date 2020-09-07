package com.darakeon.dfm.lib.api.entities.moves

import com.darakeon.dfm.testutils.LogRule
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Rule
import org.junit.Test

class NatureTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun out() {
		assertThat(Nature.get(0), `is`(Nature.Out))
	}

	@Test
	fun `in`() {
		assertThat(Nature.get(1), `is`(Nature.In))
	}

	@Test
	fun transfer() {
		assertThat(Nature.get(2), `is`(Nature.Transfer))
	}
}
