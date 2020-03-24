package com.darakeon.dfm.api

import com.darakeon.dfm.BuildConfig
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.api.CallMock
import com.darakeon.dfm.utils.TestException
import com.darakeon.dfm.utils.robolectric.assertAlertError
import com.darakeon.dfm.utils.api.internetError
import com.darakeon.dfm.utils.robolectric.simulateNetwork
import org.hamcrest.CoreMatchers.`is`
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog

@RunWith(RobolectricTestRunner::class)
class RequestHandlerTest {
	private lateinit var activity: BaseActivity
	private lateinit var handler: RequestHandler

	@Before
	fun setup() {
		activity = ActivityMock().create()
		handler = RequestHandler(activity)
	}

	@Test
	fun callOffline() {
		var result = ""

		val call = CallMock("result")
		handler.call(call) { result = it }

		call.execute()

		assertThat(result, `is`(""))

		val alert = getLatestAlertDialog()
		assertAlertError(alert, "You're offline")

		assertTrue(call.isExecuted)
	}

	@Test
	fun callOnline() {
		var result = ""

		activity.simulateNetwork()

		val call = CallMock("result")
		handler.call(call) { result = it }

		call.execute()

		assertThat(result, `is`("result"))

		assertTrue(call.isExecuted)
	}

	@Test(expected = TestException::class)
	fun callExceptionDebug() {
		if (!BuildConfig.DEBUG) throw TestException()

		activity.simulateNetwork()

		val call = CallMock(TestException())
		handler.call(call) { }

		call.execute()
	}

	@Test
	fun callExceptionRelease() {
		if (BuildConfig.DEBUG) return

		var result = ""

		activity.simulateNetwork()

		val call = CallMock(TestException())
		handler.call(call) { result = it }

		call.execute()

		assertThat(result, `is`(""))

		val alert = getLatestAlertDialog()
		assertAlertError(alert, internetError)

		assertTrue(call.isExecuted)
	}

	@Test
	fun cancel() {
		activity.simulateNetwork()

		val call = CallMock("result")
		handler.call(call) { }

		val alert = getLatestAlertDialog()
		assertNotNull(alert)
		assertTrue(alert.isShowing)

		handler.cancel(call)

		assertFalse(alert.isShowing)

		assertTrue(call.isCanceled)
	}
}
