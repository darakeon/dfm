package com.darakeon.dfm.dialogs

import android.app.Dialog
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.testutils.LogRule
import com.darakeon.dfm.testutils.robolectric.assertAlertWait
import com.darakeon.dfm.testutils.robolectric.waitTasksFinish
import com.darakeon.dfm.utils.activity.TestActivity
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertFalse
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog

@RunWith(RobolectricTestRunner::class)
class MessageTest {
	@get:Rule
	val log = LogRule()

	private lateinit var activity: BaseActivity

	@Before
	fun setup() {
		activity = ActivityMock(TestActivity::class).create()
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
		waitTasksFinish()

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
		waitTasksFinish()

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
		waitTasksFinish()

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
		waitTasksFinish()

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
		waitTasksFinish()

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
		waitTasksFinish()

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
