package com.darakeon.dfm.login

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.auth.Authentication
import com.darakeon.dfm.extensions.getPrivate
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.activity.getActivityName
import com.darakeon.dfm.utils.log.LogRule
import com.darakeon.dfm.utils.robolectric.simulateNetwork
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf

@RunWith(RobolectricTestRunner::class)
class LoginActivityTest {
	@get:Rule
	val log = LogRule()

	private lateinit var mocker: ActivityMock

	@Before
	fun setup() {
		mocker = ActivityMock()
	}

	@Test
	fun structure() {
		val activity = mocker.create<LoginActivity>()
		assertNotNull(activity.findViewById(R.id.email))
		assertNotNull(activity.findViewById(R.id.password))
		assertNotNull(activity.findViewById(R.id.login_button))
	}

	@Test
	fun login() {
		mocker.server.enqueue("login")

		val activity = mocker.create<LoginActivity>()
		activity.simulateNetwork()

		val view = View(activity)

		activity.login(view)

		val auth = activity.getPrivate<Authentication>("auth")
		assertTrue(auth.isLoggedIn)

		val shadow = shadowOf(activity)
		val intent = shadow.peekNextStartedActivity()

		assertThat(intent.getActivityName(), `is`("WelcomeActivity"))
	}
}
