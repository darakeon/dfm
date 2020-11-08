package com.darakeon.dfm.welcome

import com.darakeon.dfm.R
import com.darakeon.dfm.lib.auth.setValue
import com.darakeon.dfm.testutils.LogRule
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf

@RunWith(RobolectricTestRunner::class)
class WelcomeActivityTest {
	@get:Rule
	val log = LogRule()

	private lateinit var mocker: ActivityMock<WelcomeActivity>

	@Before
	fun setup() {
		mocker = ActivityMock(WelcomeActivity::class)
	}

	@Test
	fun structure() {
		val activity = mocker.create()
		activity.waitTasks(mocker.server)

		assertNotNull(activity.findViewById(R.id.pig))
		assertNotNull(activity.findViewById(R.id.action_logout))
		assertNotNull(activity.findViewById(R.id.action_close))
		assertNotNull(activity.findViewById(R.id.action_home))
		assertNotNull(activity.findViewById(R.id.action_move))
		assertNotNull(activity.findViewById(R.id.action_settings))
	}

	@Test
	fun exit() {
		val activity = mocker.get()
		activity.intent.putExtra("EXIT", true)

		activity.onCreate(null, null)

		assertTrue(activity.isFinishing)
	}

	@Test
	fun redirectLoggedIn() {
		val activity = mocker.get()
		activity.setValue("Ticket", "ticket")
		activity.simulateNetwork()

		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

		val shadow = shadowOf(activity)
		val intent = shadow.peekNextStartedActivity()
		assertNull(intent)
	}

	@Test
	fun redirectLoggedOut() {
		val activity = mocker.get()
		activity.setValue("Ticket", "")
		activity.simulateNetwork()

		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

		val shadow = shadowOf(activity)
		val intent = shadow.peekNextStartedActivity()
		val name = intent.getCalledName()
		assertThat(name, `is`("LoginActivity"))
	}
}
