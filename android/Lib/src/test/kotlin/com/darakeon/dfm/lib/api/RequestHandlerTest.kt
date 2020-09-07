package com.darakeon.dfm.lib.api

import com.darakeon.dfm.lib.BuildConfig
import com.darakeon.dfm.lib.extensions.getPrivate
import com.darakeon.dfm.testutils.LogRule
import com.darakeon.dfm.testutils.TestException
import com.darakeon.dfm.testutils.api.internetError
import com.darakeon.dfm.lib.utils.ActivityMock
import com.darakeon.dfm.lib.utils.ApiActivity
import com.darakeon.dfm.lib.utils.CallMock
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class RequestHandlerTest {
	@get:Rule
	val log = LogRule()

	private lateinit var activity: ApiActivity
	private lateinit var handler: RequestHandler<ApiActivity>

	@Before
	fun setup() {
		activity = ActivityMock(ApiActivity::class).create()
		handler = activity.getPrivate("api", "requestHandler")
	}

	@Test
	fun callOffline() {
		var result = ""

		val call = CallMock("result")
		handler.call(call) { result = it }

		call.execute()

		assertThat(result, `is`(""))

		assertThat(activity.errorText, `is`("You're offline"))

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
		assertThat(activity.errorText, `is`(internetError))
		assertTrue(call.isExecuted)
	}

	@Test
	fun cancel() {
		activity.simulateNetwork()

		val call = CallMock("result")
		handler.call(call) { }

		assertTrue(activity.waitStarted)

		handler.cancel(call)

		assertTrue(activity.waitEnded)

		assertTrue(call.isCanceled)
	}
}
