package com.darakeon.dfm.tfa

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.activity.getActivityName
import com.darakeon.dfm.utils.robolectric.simulateNetwork
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNotNull
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf

@RunWith(RobolectricTestRunner::class)
class TFAActivityTest {
	private lateinit var mocker: ActivityMock

	@Before
	fun setup() {
		mocker = ActivityMock()
	}

	@Test
	fun structure() {
		val activity = mocker.create<TFAActivity>()
		assertNotNull(activity.findViewById(R.id.code))
	}

	@Test
	fun verify() {
		mocker.server.enqueue("empty")

		val activity = mocker.create<TFAActivity>()
		activity.simulateNetwork()

		val view = View(activity)

		activity.verify(view)

		val shadow = shadowOf(activity)
		val intent = shadow.peekNextStartedActivity()

		assertThat(intent.getActivityName(), `is`("AccountsActivity"))
	}
}
