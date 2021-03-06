package com.darakeon.dfm.dialogs

import android.app.Dialog
import com.darakeon.dfm.R
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.robolectric.assertAlertWait
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.activity.TestActivity
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertFalse
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog

@RunWith(RobolectricTestRunner::class)
class MessageTest: BaseTest() {
	private lateinit var mocker: ActivityMock<TestActivity>
	private lateinit var activity: TestActivity

	@Before
	fun setup() {
		mocker = ActivityMock(TestActivity::class)
		activity = mocker.create()
	}

	@Test
	fun confirmClickYes() {
		var ok = false
		activity.confirm("confirm click yes") { ok = true }

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.message.toString(), `is`("confirm click yes"))

		assertTrue(alert.getButton(Dialog.BUTTON_POSITIVE).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEUTRAL).isShown)
		assertTrue(alert.getButton(Dialog.BUTTON_NEGATIVE).isShown)

		alert.getButton(Dialog.BUTTON_POSITIVE).performClick()
		activity.waitTasks(mocker.server)

		assertTrue(ok)
		assertFalse(alert.isShowing)
	}

	@Test
	fun confirmClickNo() {
		var ok = false
		activity.confirm("confirm click no") { ok = true }

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.message.toString(), `is`("confirm click no"))

		assertTrue(alert.getButton(Dialog.BUTTON_POSITIVE).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEUTRAL).isShown)
		assertTrue(alert.getButton(Dialog.BUTTON_NEGATIVE).isShown)

		alert.getButton(Dialog.BUTTON_NEGATIVE).performClick()
		activity.waitTasks(mocker.server)

		assertFalse(ok)
		assertFalse(alert.isShowing)
	}

	@Test
	fun alertErrorWithString() {
		activity.alertError("error")

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.message.toString(), `is`("error"))

		assertTrue(alert.getButton(Dialog.BUTTON_POSITIVE).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEUTRAL).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEGATIVE).isShown)
	}

	@Test
	fun alertErrorWithResId() {
		activity.alertError(R.string.app_name)

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.message.toString(), `is`("Don't fly Money"))

		assertTrue(alert.getButton(Dialog.BUTTON_POSITIVE).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEUTRAL).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEGATIVE).isShown)
	}

	@Test
	fun alertErrorWithEventClickCancel() {
		var sendError = false
		activity.alertError(R.string.app_name, R.string.ok_button) {
			sendError = true
		}

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.message.toString(), `is`("Don't fly Money"))

		assertTrue(alert.getButton(Dialog.BUTTON_POSITIVE).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEUTRAL).isShown)
		assertTrue(alert.getButton(Dialog.BUTTON_NEGATIVE).isShown)

		alert.getButton(Dialog.BUTTON_NEGATIVE).performClick()
		activity.waitTasks(mocker.server)

		assertFalse(sendError)
		assertFalse(alert.isShowing)
	}

	@Test
	fun alertErrorWithEventClickSendError() {
		var sendError = false
		activity.alertError(R.string.app_name, R.string.ok_button) {
			sendError = true
		}

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.message.toString(), `is`("Don't fly Money"))

		assertTrue(alert.getButton(Dialog.BUTTON_POSITIVE).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEUTRAL).isShown)
		assertTrue(alert.getButton(Dialog.BUTTON_NEGATIVE).isShown)

		alert.getButton(Dialog.BUTTON_POSITIVE).performClick()
		activity.waitTasks(mocker.server)

		assertTrue(sendError)
		assertFalse(alert.isShowing)
	}

	@Test
	fun createWaitDialog() {
		activity.createWaitDialog()

		val alert = getLatestAlertDialog()

		alert.assertAlertWait()

		assertFalse(alert.getButton(Dialog.BUTTON_POSITIVE).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEUTRAL).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEGATIVE).isShown)
	}
}
