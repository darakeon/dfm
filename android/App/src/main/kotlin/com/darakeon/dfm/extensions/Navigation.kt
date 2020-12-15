package com.darakeon.dfm.extensions

import android.app.Activity
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.login.LoginActivity
import com.darakeon.dfm.moves.MovesActivity
import com.darakeon.dfm.settings.SettingsActivity

const val ON_CLICK: String = "UNUSED_PARAMETER"

fun Activity.backWithExtras() {
	val extras = intent.extras ?:
		throw NavException("no extras")

	val parent = extras.get("__parent") as Class<*>
	extras.remove("__parent")

	val intent = Intent(this, parent)
	intent.putExtras(extras)

	startActivity(intent)
}

internal fun BaseActivity.logout(api: Api<BaseActivity>?) {
	if (api == null)
		logoutLocal()
	else
		api.logout(this::logoutLocal)
}

internal fun BaseActivity.logoutLocal() {
	clearAuth()
	redirect<LoginActivity>()
}

internal fun Activity.back() {
	finish()
}

internal fun Activity.goToSettings() {
	goToActivityWithBack(SettingsActivity::class.java)
}

internal fun Activity.createMove(extras: Bundle? = null) {
	goToActivityWithBack(MovesActivity::class.java, extras)
}

private fun Activity.goToActivityWithBack(newActivity: Class<*>, extras: Bundle? = null) {
	val intent = Intent(this, newActivity)

	if (extras != null)
		intent.putExtras(extras)

	intent.putExtras(this.intent)
	intent.putExtra("__parent", javaClass)

	startActivity(intent)
}

fun Activity.composeErrorEmail(url: String, error: Throwable) {
	val subject = getString(R.string.error_mail_title)

	val body = url + "\n\n" +
		error.message + "\n\n" +
		error.stackTraceText

	composeEmail(subject, body)
}

fun Activity.composeErrorApi() {
	val subject = getString(R.string.error_call_api_email)
	val body = javaClass.name
	composeEmail(subject, body)
}

internal fun Activity.contact() {
	composeEmail(
		getString(R.string.contact_subject),
		getString(R.string.contact_body)
	)
}

private fun Activity.composeEmail(subject: String, body: String) {
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
