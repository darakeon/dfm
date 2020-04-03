package com.darakeon.dfm.api

import android.app.Dialog
import android.content.Intent
import android.os.Bundle
import com.darakeon.dfm.BuildConfig
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.api.entities.Environment
import com.darakeon.dfm.auth.getValue
import com.darakeon.dfm.auth.setValue
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.api.CallMock
import com.darakeon.dfm.utils.TestException
import com.darakeon.dfm.utils.robolectric.assertAlertError
import com.darakeon.dfm.utils.execute
import com.darakeon.dfm.utils.activity.getActivityName
import com.darakeon.dfm.utils.api.internetError
import com.darakeon.dfm.utils.api.internetSlow
import com.darakeon.dfm.utils.api.noBody
import com.darakeon.dfm.utils.log.LogRule
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.startsWith
import org.junit.Assert.assertNull
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.mockito.Mockito.`when`
import org.mockito.Mockito.mock
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog
import retrofit2.Response
import java.net.ConnectException
import java.net.SocketTimeoutException

@RunWith(RobolectricTestRunner::class)
internal class ResponseHandlerTest {
	@get:Rule
	val log = LogRule()

	private lateinit var activity: BaseActivity
	private lateinit var ui: UIHandler
	private var waitEnded = false
	private lateinit var handler: ResponseHandler<String>
	private var result: String? = null

	@Before
	fun setup() {
		activity = ActivityMock().create()

		ui = mock(UIHandler::class.java)
		`when`(ui.endUIWait())
			.execute { waitEnded = true }

		handler = ResponseHandler(activity, ui) {
			result = it
		}
	}

	@Test(expected = ApiException::class)
	fun onResponse_ResponseNullDebug() {
		if (!BuildConfig.DEBUG) throw ApiException()

		handler.onResponse(CallMock(), null)
	}

	@Test
	fun onResponse_ResponseNullRelease() {
		if (BuildConfig.DEBUG) return

		handler.onResponse(CallMock(), null)

		val alert = getLatestAlertDialog()
		assertAlertError(alert, internetError)

		alert.getButton(Dialog.BUTTON_POSITIVE)
			.performClick()

		val intent = shadowOf(activity).peekNextStartedActivity()
		val extras = intent?.extras ?: Bundle()

		assertThat(extras.getString(Intent.EXTRA_TEXT), startsWith("/tests"))

		assertTrue(waitEnded)
		assertNull(result)
	}

	@Test
	fun onResponse_BodyNull() {
		val body: Body<String>? = null
		val response = Response.success(body)

		handler.onResponse(CallMock(), response)

		val alert = getLatestAlertDialog()
		assertAlertError(alert, noBody)

		assertTrue(waitEnded)
		assertNull(result)
	}

	@Test
	fun onResponse_BodyChildrenNull() {
		val body = Body<String>(null, null, null, null)
		val response = Response.success(body)

		handler.onResponse(CallMock(), response)
		val alert = getLatestAlertDialog()
		assertAlertError(alert, "Not identified site error. Contact us.")

		assertTrue(waitEnded)
		assertNull(result)
	}

	@Test
	fun onResponse_BodySuccessAndFailed() {
		val body = Body("success", null, "confusing result", 0)
		val response = Response.success(body)

		handler.onResponse(CallMock(), response)

		val alert = getLatestAlertDialog()
		assertAlertError(alert, "confusing result")

		assertTrue(waitEnded)
		assertNull(result)
	}

	@Test
	fun onResponse_ResponseBodyEnvironment() {
		val env = Environment("Light", "pt-BR")
		val body = Body("result", env, null, null)
		val response = Response.success(body)

		handler.onResponse(CallMock(), response)

		assertThat(activity.getValue("Theme").toInt(), `is`(R.style.Light))
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

		val intent = shadowOf(activity).peekNextStartedActivity()
		assertThat(intent.getActivityName(), `is`("TFAActivity"))
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

		val intent = shadowOf(activity).peekNextStartedActivity()
		assertThat(intent.getActivityName(), `is`("LoginActivity"))

		assertThat(activity.getValue("Ticket"), `is`(""))
	}

	@Test
	fun onResponse_ErrorOfAnotherType() {
		val body = Body<String>(null, null, "generic", 273)
		val response = Response.success(body)

		handler.onResponse(CallMock(), response)

		val alert = getLatestAlertDialog()
		assertAlertError(alert, "generic")

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

		handler.onFailure(CallMock(), TestException())

		val alert = getLatestAlertDialog()
		assertAlertError(alert, internetError)

		alert.getButton(Dialog.BUTTON_POSITIVE)
			.performClick()

		val intent = shadowOf(activity).peekNextStartedActivity()
		val extras = intent?.extras ?: Bundle()

		assertThat(extras.getString(Intent.EXTRA_TEXT), startsWith("/tests"))

		assertTrue(waitEnded)
		assertNull(result)
	}

	@Test
	fun onFailure_ErrorOfSocketTimeout() {
		handler.onFailure(CallMock(), SocketTimeoutException())

		val alert = getLatestAlertDialog()
		assertAlertError(alert, internetSlow)

		assertTrue(waitEnded)
		assertNull(result)
	}

	@Test
	fun onFailure_ErrorOfConnect() {
		handler.onFailure(CallMock(), ConnectException())

		val alert = getLatestAlertDialog()
		assertAlertError(alert, internetSlow)

		assertTrue(waitEnded)
		assertNull(result)
	}
}
