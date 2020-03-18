package com.darakeon.dfm.api

import android.content.pm.ActivityInfo
import android.view.WindowManager
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.robolectric.assertAlertWait
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.not
import org.junit.Assert.assertFalse
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog

@RunWith(RobolectricTestRunner::class)
class UIHandlerTest {
	private lateinit var activity: BaseActivity
	private lateinit var handler: UIHandler

	@Before
	fun setup() {
		activity = ActivityMock.create()
		handler = UIHandler(activity)
	}

	@Test
	fun startWait() {
		handler.startUIWait()

		val dialog = getLatestAlertDialog()
		assertAlertWait(dialog)

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
}
