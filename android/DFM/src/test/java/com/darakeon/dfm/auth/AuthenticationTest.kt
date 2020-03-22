package com.darakeon.dfm.auth

import com.darakeon.dfm.utils.activity.MockContext
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Before
import org.junit.Test

class AuthenticationTest {
	private lateinit var auth : Authentication

	@Before
	fun setup() {
		val mockContext = MockContext().mockSharedPreferences()
		auth = Authentication(mockContext.activity)
	}

	@Test
	fun setAndGetTicket() {
		auth.ticket = "new-ticket"
		assertThat(auth.ticket, `is`("new-ticket"))
	}

	@Test
	fun clearTicket() {
		auth.ticket = "new-ticket"
		auth.clear()
		assertThat(auth.ticket, `is`(""))
	}
}
