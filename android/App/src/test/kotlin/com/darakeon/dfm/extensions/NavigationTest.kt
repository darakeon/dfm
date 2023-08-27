package com.darakeon.dfm.extensions

import android.content.Intent
import android.os.Bundle
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.execute
import com.darakeon.dfm.utils.activity.TestBaseActivity
import com.darakeon.dfm.utils.activity.mockContext
import com.darakeon.dfm.welcome.WelcomeActivity
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.containsString
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.mockito.ArgumentMatchers.any
import org.mockito.Mockito.`when`
import org.mockito.Mockito.mock
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class NavigationTest: BaseTest() {
	private lateinit var activity: TestBaseActivity

	private var calledIntent: Intent? = null
	private val calledActivity
		get() = calledIntent?.getCalledName()

	private var calledFinish = false

	@Before
	fun setup() {
		val mockContext = mockContext()
			.mockSharedPreferences()
			.mockExternalCall()

		activity = mockContext.activity

		`when`(activity.startActivity(any()))
			.execute {
				calledIntent = it[0] as Intent
			}

		`when`(activity.finish())
			.execute { calledFinish = true }
	}

	@Test
	fun backWithExtras() {
		val intent = mock(Intent::class.java)
		`when`(activity.intent).thenReturn(intent)

		val extras = Bundle()
		`when`(intent.extras).thenReturn(extras)

		extras.putString("test", "passed")
		extras.putSerializable("__parent", WelcomeActivity::class.java)

		activity.backWithExtras()

		assertThat(calledActivity, `is`("WelcomeActivity"))

		val testExtra = calledIntent?.extras?.getString("test")
		assertThat(testExtra, `is`("passed"))
	}

	@Test(expected = NavException::class)
	fun backWithExtrasEmptyExtras() {
		val intent = mock(Intent::class.java)
		`when`(activity.intent).thenReturn(intent)
		`when`(intent.extras).thenReturn(null)

		activity.backWithExtras()
	}

	@Test
	fun logoutLocal() {
		// used this instead of verify because if someone
		// removes the "open" of the method and removes
		// the call of the other method, verify keeps
		// passing, while this one breaks
		var cleared = false
		`when`(activity.clearAuth()).execute {
			cleared = true
		}

		activity.logoutLocal()

		assertThat(calledActivity, `is`("LoginActivity"))
		assertTrue(cleared)
	}

	@Test
	fun back() {
		activity.back()
		assertTrue(calledFinish)
	}

	@Test
	fun goToSettings() {
		val intent = mock(Intent::class.java)
		`when`(activity.intent).thenReturn(intent)

		activity.goToSettings()
		assertThat(calledActivity, `is`("SettingsActivity"))

		val extras = calledIntent?.extras ?: Bundle()
		assertThat(extras["__parent"]?.toString(), containsString("BaseActivity"))
	}

	@Test
	fun createMove() {
		val intent = mock(Intent::class.java)
		`when`(activity.intent).thenReturn(intent)

		activity.createMove()
		assertThat(calledActivity, `is`("MovesActivity"))

		val extras = calledIntent?.extras ?: Bundle()
		assertThat(extras["__parent"]?.toString(), containsString("BaseActivity"))
	}

	@Test
	fun createMoveWithBundle() {
		val intent = mock(Intent::class.java)
		`when`(activity.intent).thenReturn(intent)

		val bundle = Bundle()
		bundle.putString("test", "passed")

		activity.createMove(bundle)
		assertThat(calledActivity, `is`("MovesActivity"))

		val extras = calledIntent?.extras ?: Bundle()
		assertThat(extras["__parent"]?.toString(), containsString("BaseActivity"))
		assertThat(extras["test"]?.toString(), `is`("passed"))
	}
}
