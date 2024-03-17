package com.darakeon.dfm.lib.api

import com.darakeon.dfm.lib.BuildConfig
import com.darakeon.dfm.lib.utils.ActivityMock
import com.darakeon.dfm.lib.utils.ApiActivity
import com.darakeon.dfm.lib.utils.CallMock
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.TestException
import com.darakeon.dfm.testutils.api.internetError
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.simulateOffline
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class RequestHandlerTest: BaseTest() {
	private lateinit var activity: ApiActivity
	private lateinit var handler: RequestHandler<ApiActivity>

	@Before
	fun setup() {
		activity = ActivityMock(ApiActivity::class).create()
		handler = activity.getPrivate("api", "requestHandler")
	}

	@Test
	fun callOffline() {
		var result: String? = ""

		activity.simulateOffline()

		val call = CallMock("result")
		handler.call(call, true) { result = it }

		call.execute()

		assertThat(result, `is`(""))
		assertThat(activity.errorText, `is`("There is a problem with the connection to the site"))
		assertTrue(call.isExecuted)
	}

	@Test
	fun callOnline() {
		var result: String? = ""

		activity.simulateNetwork()

		val call = CallMock("result")
		handler.call(call, true) { result = it }

		call.execute()

		assertThat(result, `is`("result"))

		assertTrue(call.isExecuted)
	}

	@Test(expected = TestException::class)
	fun callExceptionDebug() {
		if (!BuildConfig.DEBUG)
			throw TestException()

		activity.simulateNetwork()

		val call = CallMock(TestException())
		handler.call(call, false) { }

		call.execute()
	}

	@Test
	fun callExceptionRelease() {
		if (BuildConfig.DEBUG) return

		var result: String? = ""

		activity.simulateNetwork()

		val call = CallMock(TestException())
		handler.call(call, true) { result = it }

		call.execute()

		assertThat(result, `is`(""))
		assertThat(activity.errorText, `is`(internetError))
		assertTrue(call.isExecuted)
	}

	@Test
	fun cancel() {
		activity.simulateNetwork()

		val call = CallMock("result")
		handler.call(call, false) { }

		assertTrue(activity.waitStarted)

		handler.cancel(call)

		assertTrue(activity.waitEnded)

		assertTrue(call.isCanceled)
	}
}
