package com.darakeon.dfm.api.entities.moves

import org.hamcrest.core.Is.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class NatureTest {
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
