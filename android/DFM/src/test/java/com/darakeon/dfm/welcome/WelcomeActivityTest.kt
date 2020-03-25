package com.darakeon.dfm.welcome

import com.darakeon.dfm.R
import com.darakeon.dfm.auth.setValue
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.activity.getActivityName
import com.darakeon.dfm.utils.robolectric.simulateNetwork
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf

@RunWith(RobolectricTestRunner::class)
class WelcomeActivityTest {
	private lateinit var mocker: ActivityMock

	@Before
	fun setup() {
		mocker = ActivityMock()
	}

	@Test
	fun structure() {
		val activity = mocker.create<WelcomeActivity>()
		assertNotNull(activity.findViewById(R.id.pig))
	}

	@Test
	fun exit() {
		val activity = mocker.get<WelcomeActivity>()
		activity.intent.putExtra("EXIT", true)

		activity.onCreate(null, null)

		assertTrue(activity.isFinishing)
	}

	@Test
	fun redirectLoggedIn() {
		val activity = mocker.get<WelcomeActivity>()
		activity.setValue("Ticket", "ticket")
		activity.simulateNetwork()
		mocker.server.enqueue("empty")

		activity.onCreate(null, null)

		val shadow = shadowOf(activity)
		val intent = shadow.peekNextStartedActivity()
		val name = intent.getActivityName()
		assertThat(name, `is`("AccountsActivity"))
	}

	@Test
	fun redirectLoggedOut() {
		val activity = mocker.get<WelcomeActivity>()
		activity.setValue("Ticket", "")
		activity.simulateNetwork()
		mocker.server.enqueue("empty")

		activity.onCreate(null, null)

		val shadow = shadowOf(activity)
		val intent = shadow.peekNextStartedActivity()
		val name = intent.getActivityName()
		assertThat(name, `is`("LoginActivity"))
	}
}
