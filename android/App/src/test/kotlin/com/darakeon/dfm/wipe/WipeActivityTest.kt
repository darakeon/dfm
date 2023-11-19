package com.darakeon.dfm.wipe

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
class WipeActivityTest: BaseTest() {
	private lateinit var mocker: ActivityMock<WipeActivity>
	private lateinit var activity: WipeActivity

	@Before
	fun setup() {
		mocker = ActivityMock(WipeActivity::class)
		activity = mocker.get()
	}

	@After
	fun tearDown() {
		mocker.server.shutdown()
	}

	@Test
	fun structure() {
		activity.onCreate(null, null)

		assertNotNull(activity.findViewById(R.id.explanation))
		assertNotNull(activity.findViewById(R.id.password))
	}

	@Test
	fun wipe() {
		activity.simulateNetwork()

		activity.onCreate(null, null)

		mocker.server.enqueue("empty")
		activity.wipe(View(activity))
		activity.waitTasks(mocker.server)

		assertThat(activity.ticket, `is`(""))

		val shadowActivity = shadowOf(activity)
		val intent = shadowActivity.peekNextStartedActivity()

		assertThat(intent.getCalledName(), `is`("LoginActivity"))
	}
}
