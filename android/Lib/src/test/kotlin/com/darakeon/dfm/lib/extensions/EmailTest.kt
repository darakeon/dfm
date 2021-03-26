package com.darakeon.dfm.lib.extensions

import android.content.Context
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import com.darakeon.dfm.lib.R
import com.darakeon.dfm.lib.utils.mockContext
import com.darakeon.dfm.testutils.TestException
import com.darakeon.dfm.testutils.execute
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.containsString
import org.hamcrest.CoreMatchers.hasItem
import org.hamcrest.CoreMatchers.startsWith
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.mockito.ArgumentMatchers.any
import org.mockito.Mockito.`when`
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class EmailTest {
	private lateinit var activity: Context
	private var calledIntent: Intent? = null

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
	}

	@Test
	fun composeErrorEmail() {
		`when`(activity.getString(R.string.error_mail_title))
			.thenReturn("SUBJECT")

		`when`(activity.getString(R.string.dfm_mail_address))
			.thenReturn("EMAIL")

		activity.composeErrorEmail("url", TestException("error"))

		assertThat(calledIntent?.action, `is`(Intent.ACTION_SENDTO))
		assertThat(calledIntent?.data, `is`(Uri.parse("mailto:")))

		val extras = calledIntent?.extras ?: Bundle()

		assertThat(
			extras.getStringArray(Intent.EXTRA_EMAIL)?.toList(),
			hasItem("EMAIL")
		)

		assertThat(
			extras.getString(Intent.EXTRA_SUBJECT),
			`is`("SUBJECT")
		)

		assertThat(
			extras.getString(Intent.EXTRA_TEXT),
			startsWith("url\n\nerror\n\ncom.darakeon.dfm.lib.extensions.EmailTest.composeErrorEmail")
		)
	}

	@Test
	fun composeErrorApi() {
		`when`(activity.getString(R.string.error_call_api_email))
			.thenReturn("API")

		`when`(activity.getString(R.string.dfm_mail_address))
			.thenReturn("EMAIL")

		activity.composeErrorApi()

		assertThat(calledIntent?.action, `is`(Intent.ACTION_SENDTO))
		assertThat(calledIntent?.data, `is`(Uri.parse("mailto:")))

		val extras = calledIntent?.extras ?: Bundle()

		assertThat(
			extras.getStringArray(Intent.EXTRA_EMAIL)?.toList(),
			hasItem("EMAIL")
		)

		assertThat(
			extras.getString(Intent.EXTRA_SUBJECT),
			`is`("API")
		)

		assertThat(
			extras.getString(Intent.EXTRA_TEXT),
			containsString("android.app.Activity")
		)
	}

	@Test
	fun contact() {
		`when`(activity.getString(R.string.dfm_mail_address))
			.thenReturn("ADDRESS")

		`when`(activity.getString(R.string.contact_subject))
			.thenReturn("SUBJECT")

		`when`(activity.getString(R.string.contact_body))
			.thenReturn("BODY")

		activity.contact()

		assertThat(calledIntent?.action, `is`(Intent.ACTION_SENDTO))
		assertThat(calledIntent?.data, `is`(Uri.parse("mailto:")))

		val extras = calledIntent?.extras ?: Bundle()

		assertThat(
			extras.getStringArray(Intent.EXTRA_EMAIL)?.toList(),
			hasItem("ADDRESS")
		)

		assertThat(
			extras.getString(Intent.EXTRA_SUBJECT),
			`is`("SUBJECT")
		)

		assertThat(
			extras.getString(Intent.EXTRA_TEXT),
			startsWith("BODY")
		)
	}
}
