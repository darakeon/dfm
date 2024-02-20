package com.darakeon.dfm.signup

import android.view.View
import android.widget.Button
import com.darakeon.dfm.R
import com.darakeon.dfm.lib.auth.Authentication
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.After
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf

@RunWith(RobolectricTestRunner::class)
class SignUpActivityTest: BaseTest() {
	private lateinit var mocker: ActivityMock<SignUpActivity>

	@Before
	fun setup() {
		mocker = ActivityMock(SignUpActivity::class)
	}

	@After
	fun tearDown() {
		mocker.server.shutdown()
	}

	@Test
	fun structure() {
		val activity = mocker.create()

		assertNotNull(activity.findViewById(R.id.email))
		assertNotNull(activity.findViewById(R.id.password))
		assertNotNull(activity.findViewById(R.id.accept_terms))
	}

	@Test
	fun signUp() {
		mocker.server.enqueue("signup")
		mocker.server.enqueue("login")

		val activity = mocker.create()
		activity.simulateNetwork()

		val view = View(activity)

		activity.signUp(view)
		activity.waitTasks(mocker.server)

		val auth = activity.getPrivate<Authentication>("auth")
		assertTrue(auth.isLoggedIn)

		val shadow = shadowOf(activity)
		val intent = shadow.peekNextStartedActivity()

		assertThat(intent.getCalledName(), `is`("WelcomeActivity"))
	}

	@Test
	fun goToTerms() {
		val activity = mocker.create()

		val link = activity.findViewById<Button>(R.id.terms_link)
		link.performClick()

		val called = shadowOf(activity)
			.peekNextStartedActivity()
			.getCalledName()

		assertThat(called, `is`("TermsActivity"))
	}
}
