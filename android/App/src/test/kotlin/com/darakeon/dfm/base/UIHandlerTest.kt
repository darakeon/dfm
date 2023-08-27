package com.darakeon.dfm.base

import android.content.pm.ActivityInfo
import android.view.WindowManager
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.robolectric.assertAlertWait
import com.darakeon.dfm.utils.activity.TestActivity
import com.darakeon.dfm.utils.activity.TestBaseActivity
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.not
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNull
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog

@RunWith(RobolectricTestRunner::class)
class UIHandlerTest: BaseTest() {
	private lateinit var activity: TestBaseActivity
	private lateinit var handler: UIHandler

	@Before
	fun setup() {
		activity = ActivityMock(TestActivity::class).create()
		handler = UIHandler(activity)
	}

	@Test
	fun startWait() {
		handler.startUIWait()

		getLatestAlertDialog()
			.assertAlertWait()

		val screenOn = WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON
		assertThat(
			activity.window.attributes.flags.and(screenOn),
			`is`(screenOn)
		)

		assertThat(
			activity.requestedOrientation,
			not(`is`(ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED))
		)
	}

	@Test
	fun endWait() {
		handler.startUIWait()
		handler.endUIWait()

		val dialog = getLatestAlertDialog()
		assertFalse(dialog.isShowing)

		val screenOn = WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON
		assertThat(
			activity.window.attributes.flags.and(screenOn),
			`is`(0)
		)

		assertThat(
			activity.requestedOrientation,
			`is`(ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED)
		)
	}

	@Test
	fun startAfterEndWait() {
		handler.endUIWait()
		handler.startUIWait()

		val dialog = getLatestAlertDialog()
		assertNull(dialog)

		val screenOn = WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON
		assertThat(
			activity.window.attributes.flags.and(screenOn),
			`is`(0)
		)

		assertThat(
			activity.requestedOrientation,
			`is`(ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED)
		)
	}

	@Test
	fun startEndAndStartAgain() {
		handler.startUIWait()
		handler.endUIWait()
		handler.startUIWait()

		getLatestAlertDialog()
			.assertAlertWait()

		val screenOn = WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON
		assertThat(
			activity.window.attributes.flags.and(screenOn),
			`is`(screenOn)
		)

		assertThat(
			activity.requestedOrientation,
			not(`is`(ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED))
		)
	}
}
