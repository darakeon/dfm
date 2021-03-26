package com.darakeon.dfm.lib.extensions

import android.content.Context
import android.content.Intent
import android.net.Uri
import com.darakeon.dfm.lib.R

fun Context.composeErrorApi() {
	val subject = getString(R.string.error_call_api_email)
	val body = javaClass.name
	composeEmail(subject, body)
}

fun Context.contact() {
	composeEmail(
		getString(R.string.contact_subject),
		getString(R.string.contact_body)
	)
}

fun Context.composeErrorEmail(url: String, error: Throwable) {
	val subject = getString(R.string.error_mail_title)

	val body = url + "\n\n" +
		error.message + "\n\n" +
		error.stackTraceText

	composeEmail(subject, body)
}

fun Context.composeEmail(subject: String, body: String) {
	val intent = Intent(Intent.ACTION_SENDTO)

	// only email apps should handle this
	intent.data = Uri.parse("mailto:")

	val email = arrayOf(
		getString(R.string.dfm_mail_address)
	)

	intent.putExtra(Intent.EXTRA_EMAIL, email)
	intent.putExtra(Intent.EXTRA_SUBJECT, subject)
	intent.putExtra(Intent.EXTRA_TEXT, body)

	startActivity(intent)
}
