package com.darakeon.dfm.tfa

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.After
import org.junit.Assert.assertNotNull
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf

@RunWith(RobolectricTestRunner::class)
class TFAActivityTest: BaseTest() {
	private lateinit var mocker: ActivityMock<TFAActivity>

	@Before
	fun setup() {
		mocker = ActivityMock(TFAActivity::class)
	}

	@After
	fun tearDown() {
		mocker.server.shutdown()
	}

	@Test
	fun structure() {
		val activity = mocker.create()

		assertNotNull(activity.findViewById(R.id.code))
	}

	@Test
	fun verify() {
		mocker.server.enqueue("empty")

		val activity = mocker.create()
		activity.simulateNetwork()

		val view = View(activity)

		activity.verify(view)
		activity.waitTasks(mocker.server)

		val shadow = shadowOf(activity)
		val intent = shadow.peekNextStartedActivity()

		assertThat(intent.getCalledName(), `is`("AccountsActivity"))
	}
}
