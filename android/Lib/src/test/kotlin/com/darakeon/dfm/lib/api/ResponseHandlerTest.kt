package com.darakeon.dfm.lib.api

import com.darakeon.dfm.lib.BuildConfig
import com.darakeon.dfm.lib.R
import com.darakeon.dfm.lib.api.entities.Body
import com.darakeon.dfm.lib.api.entities.Environment
import com.darakeon.dfm.lib.api.entities.Theme
import com.darakeon.dfm.lib.auth.getValue
import com.darakeon.dfm.lib.auth.setValue
import com.darakeon.dfm.lib.utils.ActivityMock
import com.darakeon.dfm.lib.utils.ApiActivity
import com.darakeon.dfm.lib.utils.CallMock
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.TestException
import com.darakeon.dfm.testutils.api.internetError
import com.darakeon.dfm.testutils.api.internetSlow
import com.darakeon.dfm.testutils.api.noBody
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog
import retrofit2.Response
import java.net.ConnectException
import java.net.SocketTimeoutException

@RunWith(RobolectricTestRunner::class)
internal class ResponseHandlerTest: BaseTest() {
	private lateinit var activity: ApiActivity
	private lateinit var handler: ResponseHandler<ApiActivity, String>
	private var result: String? = null

	private val waitEnded
		get() = activity.waitEnded

	@Before
	fun setup() {
		activity = ActivityMock(ApiActivity::class).create()

		handler = ResponseHandler(activity) {
			result = it
		}
	}

	@Test
	fun onResponse_BodyNull() {
		val body: Body<String>? = null
		val response = Response.success(body)

		handler.onResponse(CallMock(), response)

		assertThat(
			activity.errorText,
			`is`(noBody)
		)

		assertTrue(waitEnded)
		assertNull(result)
	}

	@Test
	fun onResponse_BodyChildrenNull() {
		val body = Body<String>(null, null, null, null)
		val response = Response.success(body)

		handler.onResponse(CallMock(), response)

		assertThat(
			activity.errorText,
			`is`("Not identified site error. Contact us.")
		)

		assertTrue(waitEnded)
		assertNull(result)
	}

	@Test
	fun onResponse_BodySuccessAndFailed() {
		val body = Body("success", null, "confusing result", 0)
		val response = Response.success(body)

		handler.onResponse(CallMock(), response)

		assertThat(
			activity.errorText,
			`is`("confusing result")
		)

		assertTrue(waitEnded)
		assertNull(result)
	}

	@Test
	fun onResponse_ResponseBodyEnvironment() {
		val env = Environment(Theme.LightNature, "pt-BR")
		val body = Body("result", env, null, null)
		val response = Response.success(body)

		handler.onResponse(CallMock(), response)

		assertThat(activity.getValue("Theme").toInt(), `is`(R.style.LightNature))
		assertThat(activity.getValue("Language"), `is`("pt_BR"))

		assertTrue(waitEnded)
		assertThat(result, `is`("result"))
	}

	@Test
	fun onResponse_BodySuccess() {
		val body = Body("result", null, null, null)
		val response = Response.success(body)

		handler.onResponse(CallMock(), response)

		assertTrue(waitEnded)
		assertThat(result, `is`("result"))
	}

	@Test
	fun onResponse_ErrorOfTfa() {
		val errorCode = activity.resources.getInteger(R.integer.TFA)
		val body = Body<String>(null, null, "TFA", errorCode)
		val response = Response.success(body)

		handler.onResponse(CallMock(), response)

		val alert = getLatestAlertDialog()
		assertNull(alert)

		assertTrue(waitEnded)
		assertNull(result)

		assertTrue(activity.checkedTFA)
	}

	@Test
	fun onResponse_ErrorOfUninvited() {
		val errorCode = activity.resources.getInteger(R.integer.uninvited)
		val body = Body<String>(null, null, "TFA", errorCode)
		val response = Response.success(body)

		activity.setValue("Ticket", "fake")

		handler.onResponse(CallMock(), response)

		val alert = getLatestAlertDialog()
		assertNull(alert)

		assertTrue(waitEnded)
		assertNull(result)

		assertTrue(activity.loggedOut)
	}

	@Test
	fun onResponse_ErrorOfAnotherType() {
		val body = Body<String>(null, null, "generic", 273)
		val response = Response.success(body)

		handler.onResponse(CallMock(), response)

		assertThat(activity.errorText, `is`("generic"))

		assertTrue(waitEnded)
		assertNull(result)
	}

	@Test(expected = TestException::class)
	fun onFailure_ErrorCommonDebug() {
		if (!BuildConfig.DEBUG) throw TestException()

		handler.onFailure(CallMock(), TestException())
	}

	@Test
	fun onFailure_ErrorCommonRelease() {
		if (BuildConfig.DEBUG) return

		val error = TestException()

		handler.onFailure(CallMock(), error)

		assertThat(activity.errorText, `is`(internetError))
		assertThat(activity.error, `is`(error))

		assertTrue(waitEnded)
		assertNull(result)
	}

	@Test
	fun onFailure_ErrorOfSocketTimeout() {
		handler.onFailure(CallMock(), SocketTimeoutException())

		assertThat(
			activity.errorText,
			`is`(internetSlow)
		)

		assertTrue(waitEnded)
		assertNull(result)
	}

	@Test
	fun onFailure_ErrorOfConnect() {
		handler.onFailure(CallMock(), ConnectException())

		assertThat(
			activity.errorText,
			`is`(internetSlow)
		)

		assertTrue(waitEnded)
		assertNull(result)
	}
}
