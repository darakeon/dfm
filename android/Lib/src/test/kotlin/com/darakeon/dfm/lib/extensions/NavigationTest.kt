package com.darakeon.dfm.lib.extensions

import android.app.Activity
import android.content.Intent
import com.darakeon.dfm.lib.utils.mockContext
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.execute
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.mockito.ArgumentMatchers.any
import org.mockito.Mockito.`when`
import org.mockito.Mockito.mock
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class NavigationTest: BaseTest() {
	private lateinit var activity: Activity
	private var calledIntent: Intent? = null
	private val calledActivity
		get() = calledIntent?.getCalledName()

	@Before
	fun setup() {
		activity = mockContext().activity
		`when`(activity.startActivity(any()))
			.execute { calledIntent = it[0] as Intent }
	}

	@Test
	fun refresh() {
		val intent = mock(Intent::class.java)
		`when`(activity.intent).thenReturn(intent)

		activity.refresh()

		assertThat(calledIntent, `is`(intent))
	}

	@Test
	fun redirect() {
		activity.redirect<Activity>()
		assertThat(calledActivity, `is`("Activity"))
	}

	@Test
	fun redirectWithChangeIntent() {
		var intentModifierCalled = false
		activity.redirect<Activity> {
			intentModifierCalled = true
		}
		assertThat(calledActivity, `is`("Activity"))
		Assert.assertTrue(intentModifierCalled)
	}
}
