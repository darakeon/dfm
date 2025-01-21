package com.darakeon.dfm.lib.api

import com.darakeon.dfm.lib.BuildConfig
import com.darakeon.dfm.lib.R
import com.darakeon.dfm.lib.api.entities.Body
import com.darakeon.dfm.lib.api.entities.Environment
import com.darakeon.dfm.lib.api.entities.Error
import com.darakeon.dfm.lib.api.entities.Theme
import com.darakeon.dfm.lib.api.entities.ThemeEnum
import com.darakeon.dfm.lib.api.entities.status.StatusEnum
import com.darakeon.dfm.lib.api.entities.status.StatusResponse
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
import okhttp3.ResponseBody.Companion.toResponseBody
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

	private var notFound: Boolean = false

	private lateinit var handlerNoData: ResponseHandler<ApiActivity, String>

	private lateinit var handlerText: ResponseHandler<ApiActivity, String>
	private var resultText: String? = null

	private lateinit var handlerStatus: ResponseHandler<ApiActivity, StatusResponse>
	private var resultStatus: StatusResponse? = null

	private val waitEnded
		get() = activity.waitEnded

	@Before
	fun setup() {
		activity = ActivityMock(ApiActivity::class).create()

		notFound = false

		handlerText = ResponseHandler(activity, true, { notFound = true }) {
			resultText = it
		}

		handlerNoData = ResponseHandler(activity, false, { notFound = true }) {}

		handlerStatus = ResponseHandler(activity, false, { notFound = true }) {
			resultStatus = it
		}
	}

	@Test
	fun onResponseData_BodyNull() {
		val body: Body<String>? = null
		val response = Response.success(body)

		handlerText.onResponse(CallMock.ForString(), response)

		assertThat(
			activity.errorText,
			`is`(noBody)
		)

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onResponseNoData_BodyNull() {
		val body: Body<String>? = null
		val response = Response.success(body)

		handlerNoData.onResponse(CallMock.ForString(), response)

		assertThat(
			activity.errorText,
			`is`(noBody)
		)

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onResponseData_BodyChildrenNull() {
		val body = Body<String>(null, null, null)
		val response = Response.success(body)

		handlerText.onResponse(CallMock.ForString(), response)

		assertThat(
			activity.errorText,
			`is`("Not identified site error. Contact us.")
		)

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onResponseNoData_BodyChildrenNull() {
		val body = Body<String>(null, null, null)
		val response = Response.success(body)

		handlerNoData.onResponse(CallMock.ForString(), response)

		assertNull(activity.errorText)
		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onResponseData_BodySuccessAndFailed() {
		val body = Body("success", null, Error(0, "confusing result"))
		val response = Response.success(body)

		handlerText.onResponse(CallMock.ForString(), response)

		assertThat(
			activity.errorText,
			`is`("confusing result")
		)

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onResponseNoData_BodySuccessAndFailed() {
		val body = Body("success", null, Error(0, "confusing result"))
		val response = Response.success(body)

		handlerNoData.onResponse(CallMock.ForString(), response)

		assertThat(
			activity.errorText,
			`is`("confusing result")
		)

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onResponseData_ResponseBodyEnvironment() {
		val env = Environment(ThemeEnum(Theme.LightNature.code), "pt-BR")
		val body = Body("result", env, null)
		val response = Response.success(body)

		handlerText.onResponse(CallMock.ForString(), response)

		assertThat(activity.getValue("Theme").toInt(), `is`(R.style.LightNature))
		assertThat(activity.getValue("Language"), `is`("pt_BR"))

		assertTrue(waitEnded)
		assertThat(resultText, `is`("result"))
	}

	@Test
	fun onResponseNoData_ResponseBodyEnvironment() {
		val env = Environment(ThemeEnum(Theme.LightNature.code), "pt-BR")
		val body = Body("result", env, null)
		val response = Response.success(body)

		handlerNoData.onResponse(CallMock.ForString(), response)

		assertThat(activity.getValue("Theme").toInt(), `is`(R.style.LightNature))
		assertThat(activity.getValue("Language"), `is`("pt_BR"))

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onResponseData_BodySuccess() {
		val body = Body("result", null, null)
		val response = Response.success(body)

		handlerText.onResponse(CallMock.ForString(), response)

		assertTrue(waitEnded)
		assertThat(resultText, `is`("result"))
	}

	@Test
	fun onResponseNoData_BodySuccess() {
		val body = Body("result", null, null)
		val response = Response.success(body)

		handlerNoData.onResponse(CallMock.ForString(), response)

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onResponseData_ErrorOfTfa() {
		val errorCode = activity.resources.getInteger(R.integer.TFA)
		val body = Body<String>(null, null, Error(errorCode, "TFA"))
		val response = Response.success(body)

		handlerText.onResponse(CallMock.ForString(), response)

		val alert = getLatestAlertDialog()
		assertNull(alert)

		assertTrue(waitEnded)
		assertNull(resultText)

		assertTrue(activity.checkedTFA)
	}

	@Test
	fun onResponseNoData_ErrorOfTfa() {
		val errorCode = activity.resources.getInteger(R.integer.TFA)
		val body = Body<String>(null, null, Error(errorCode, "TFA"))
		val response = Response.success(body)

		handlerNoData.onResponse(CallMock.ForString(), response)

		val alert = getLatestAlertDialog()
		assertNull(alert)

		assertTrue(waitEnded)
		assertNull(resultText)

		assertTrue(activity.checkedTFA)
	}

	@Test
	fun onResponseData_ErrorOfUninvited() {
		val errorCode = activity.resources.getInteger(R.integer.uninvited)
		val body = Body<String>(null, null, Error(errorCode, "TFA"))
		val response = Response.success(body)

		activity.setValue("Ticket", "fake")

		handlerText.onResponse(CallMock.ForString(), response)

		val alert = getLatestAlertDialog()
		assertNull(alert)

		assertTrue(waitEnded)
		assertNull(resultText)

		assertTrue(activity.loggedOut)
	}

	@Test
	fun onResponseNoData_ErrorOfUninvited() {
		val errorCode = activity.resources.getInteger(R.integer.uninvited)
		val body = Body<String>(null, null, Error(errorCode, "TFA"))
		val response = Response.success(body)

		activity.setValue("Ticket", "fake")

		handlerNoData.onResponse(CallMock.ForString(), response)

		val alert = getLatestAlertDialog()
		assertNull(alert)

		assertTrue(waitEnded)
		assertNull(resultText)

		assertTrue(activity.loggedOut)
	}

	@Test
	fun onResponseData_ErrorOfAnotherType() {
		val body = Body<String>(null, null, Error(273, "generic"))
		val response = Response.success(body)

		handlerText.onResponse(CallMock.ForString(), response)

		assertThat(activity.errorText, `is`("generic"))

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onResponseNoData_ErrorOfAnotherType() {
		val body = Body<String>(null, null, Error(273, "generic"))
		val response = Response.success(body)

		handlerNoData.onResponse(CallMock.ForString(), response)

		assertThat(activity.errorText, `is`("generic"))

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onResponse_StatusNone() {
		val body = Body(StatusResponse(StatusEnum(0)), null, null)
		val response = Response.success(body)

		handlerStatus.onResponse(CallMock.ForStatusResponse(), response)

		assertThat(activity.errorText, `is`("Error on contacting the Pig. The error is recorded and we will solve it as soon as possible."))

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onResponse_StatusDbError() {
		val body = Body(StatusResponse(StatusEnum(2)), null, null)
		val response = Response.success(body)

		handlerStatus.onResponse(CallMock.ForStatusResponse(), response)

		assertThat(activity.errorText, `is`("Error on contacting the Pig. There is a error accessing data. The error is recorded and we will solve it as soon as possible."))

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onResponse_StatusMaintenance() {
		val body = Body(StatusResponse(StatusEnum(3)), null, null)
		val response = Response.success(body)

		handlerStatus.onResponse(CallMock.ForStatusResponse(), response)

		assertThat(activity.errorText, `is`("Error on contacting the Pig. The System is in the maintenance window. To more details, visit the website."))

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onResponse_NotFound() {
		val body = "".toResponseBody()
		val response = Response.error<Body<String>>(404, body)

		handlerNoData.onResponse(CallMock.ForString(), response)

		assertNull(activity.errorText)

		assertTrue(waitEnded)
		assertNull(resultText)
		assertTrue(notFound)
	}

	@Test(expected = TestException::class)
	fun onFailureData_ErrorCommonDebug() {
		if (!BuildConfig.DEBUG) throw TestException()

		handlerText.onFailure(CallMock.ForString(), TestException())
	}

	@Test(expected = TestException::class)
	fun onFailureNoData_ErrorCommonDebug() {
		if (!BuildConfig.DEBUG) throw TestException()

		handlerNoData.onFailure(CallMock.ForString(), TestException())
	}

	@Test
	fun onFailureData_ErrorCommonRelease() {
		if (BuildConfig.DEBUG) return

		val error = TestException()

		handlerText.onFailure(CallMock.ForString(), error)

		assertThat(activity.errorText, `is`(internetError))
		assertThat(activity.error, `is`(error))

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onFailureNoData_ErrorCommonRelease() {
		if (BuildConfig.DEBUG) return

		val error = TestException()

		handlerNoData.onFailure(CallMock.ForString(), error)

		assertThat(activity.errorText, `is`(internetError))
		assertThat(activity.error, `is`(error))

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onFailureData_ErrorOfSocketTimeout() {
		handlerText.onFailure(CallMock.ForString(), SocketTimeoutException())

		assertThat(
			activity.errorText,
			`is`(internetSlow)
		)

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onFailureNoData_ErrorOfSocketTimeout() {
		handlerNoData.onFailure(CallMock.ForString(), SocketTimeoutException())

		assertThat(
			activity.errorText,
			`is`(internetSlow)
		)

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onFailureData_ErrorOfConnect() {
		handlerText.onFailure(CallMock.ForString(), ConnectException())

		assertThat(
			activity.errorText,
			`is`(internetSlow)
		)

		assertTrue(waitEnded)
		assertNull(resultText)
	}

	@Test
	fun onFailureNoData_ErrorOfConnect() {
		handlerNoData.onFailure(CallMock.ForString(), ConnectException())

		assertThat(
			activity.errorText,
			`is`(internetSlow)
		)

		assertTrue(waitEnded)
		assertNull(resultText)
	}
}
