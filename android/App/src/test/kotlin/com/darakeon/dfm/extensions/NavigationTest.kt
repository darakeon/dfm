package com.darakeon.dfm.extensions

import android.content.Intent
import android.net.Uri
import android.os.Bundle
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.TestException
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.execute
import com.darakeon.dfm.utils.activity.mockContext
import com.darakeon.dfm.welcome.WelcomeActivity
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.containsString
import org.hamcrest.CoreMatchers.hasItem
import org.hamcrest.CoreMatchers.startsWith
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
	private lateinit var activity: BaseActivity

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
	fun close() {
		activity.close()

		assertThat(calledActivity, `is`("WelcomeActivity"))

		assertThat(calledIntent?.flags, `is`(
			Intent.FLAG_ACTIVITY_CLEAR_TOP +
				Intent.FLAG_ACTIVITY_CLEAR_TASK
		))

		assertTrue(calledIntent?.extras?.getBoolean("EXIT") ?: false)
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

	@Test
	fun composeErrorEmail() {
		`when`(activity.getString(R.string.error_mail_title))
			.thenReturn("SUBJECT")

		`when`(activity.getString(R.string.error_mail_address))
			.thenReturn("EMAIL")

		activity.composeErrorEmail("url", TestException("error"))

		assertThat(calledIntent?.action, `is`(Intent.ACTION_SENDTO))
		assertThat(calledIntent?.data, `is`(Uri.parse("mailto:")))

		val extras = calledIntent?.extras ?: Bundle()

		assertThat(
			extras.getStringArray(Intent.EXTRA_EMAIL)?.toList(),
			hasItem("EMAIL")
		)

		assertThat(extras.getString(Intent.EXTRA_SUBJECT), `is`("SUBJECT"))

		assertThat(
			extras.getString(Intent.EXTRA_TEXT),
			startsWith("url\n\nerror\n\ncom.darakeon.dfm.extensions.NavigationTest.composeErrorEmail")
		)
	}

	@Test
	fun composeErrorApi() {
		`when`(activity.getString(R.string.error_call_api_email))
			.thenReturn("API")

		`when`(activity.getString(R.string.error_mail_address))
			.thenReturn("EMAIL")

		activity.composeErrorApi()

		assertThat(calledIntent?.action, `is`(Intent.ACTION_SENDTO))
		assertThat(calledIntent?.data, `is`(Uri.parse("mailto:")))

		val extras = calledIntent?.extras ?: Bundle()

		assertThat(
			extras.getStringArray(Intent.EXTRA_EMAIL)?.toList(),
			hasItem("EMAIL")
		)

		assertThat(extras.getString(Intent.EXTRA_SUBJECT), `is`("API"))
		assertThat(extras.getString(Intent.EXTRA_TEXT), containsString("BaseActivity"))
	}
}
