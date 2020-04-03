package com.darakeon.dfm.auth

import com.darakeon.dfm.utils.activity.MockContext
import com.darakeon.dfm.utils.log.LogRule
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertFalse
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Rule
import org.junit.Test

class AuthenticationTest {
	@get:Rule
	val log = LogRule()

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

	@Test
	fun isLoggedIn() {
		auth.ticket = "new-ticket"
		assertTrue(auth.isLoggedIn)
	}

	@Test
	fun isLoggedOut() {
		auth.ticket = "new-ticket"
		auth.clear()
		assertFalse(auth.isLoggedIn)
	}
}
