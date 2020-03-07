package com.darakeon.dfm.dialogs

import android.app.Dialog
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.utils.ActivityMock
import com.darakeon.dfm.utils.assertAlertWait
import org.hamcrest.CoreMatchers.`is`
import org.junit.Assert.assertFalse
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog

@RunWith(RobolectricTestRunner::class)
class MessageTest {
	private lateinit var activity: BaseActivity

	@Before
	fun setup() {
		activity = ActivityMock.create()
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

		alert.getButton(Dialog.BUTTON_POSITIVE).performClick()
		assertFalse(alert.isShowing)
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

		alert.getButton(Dialog.BUTTON_POSITIVE).performClick()
		assertFalse(alert.isShowing)
	}

	@Test
	fun alertErrorWithEventClickCancel() {
		var sendError = false
		activity.alertError(R.string.app_name) { sendError = true }

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.message.toString(), `is`("Don't fly Money"))

		assertTrue(alert.getButton(Dialog.BUTTON_POSITIVE).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEUTRAL).isShown)
		assertTrue(alert.getButton(Dialog.BUTTON_NEGATIVE).isShown)

		alert.getButton(Dialog.BUTTON_NEGATIVE).performClick()

		assertFalse(sendError)
		assertFalse(alert.isShowing)
	}

	@Test
	fun alertErrorWithEventClickSendError() {
		var sendError = false
		activity.alertError(R.string.app_name) { sendError = true }

		val alert = getLatestAlertDialog()
		assertTrue(alert.isShowing)

		val shadow = shadowOf(alert)
		assertThat(shadow.message.toString(), `is`("Don't fly Money"))

		assertTrue(alert.getButton(Dialog.BUTTON_POSITIVE).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEUTRAL).isShown)
		assertTrue(alert.getButton(Dialog.BUTTON_NEGATIVE).isShown)

		alert.getButton(Dialog.BUTTON_POSITIVE).performClick()

		assertTrue(sendError)
		assertFalse(alert.isShowing)
	}

	@Test
	fun createWaitDialog() {
		activity.createWaitDialog()

		val alert = getLatestAlertDialog()
		assertAlertWait(alert)

		assertFalse(alert.getButton(Dialog.BUTTON_POSITIVE).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEUTRAL).isShown)
		assertFalse(alert.getButton(Dialog.BUTTON_NEGATIVE).isShown)
	}
}
