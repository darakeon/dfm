package com.darakeon.dfm.api.entities.moves

import com.darakeon.dfm.utils.log.LogRule
import org.hamcrest.core.Is.`is`
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
