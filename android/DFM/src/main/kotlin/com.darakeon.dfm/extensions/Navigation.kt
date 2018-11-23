package com.darakeon.dfm.extensions

import android.app.Activity
import android.content.Context
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import com.darakeon.dfm.R
import com.darakeon.dfm.api.Api
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.login.LoginActivity
import com.darakeon.dfm.moves.MovesCreateActivity
import com.darakeon.dfm.settings.SettingsActivity
import com.darakeon.dfm.welcome.WelcomeActivity

const val ON_CLICK: String = "UNUSED_PARAMETER"

inline fun <reified T : Activity> Context.redirect(
	function: (Intent) -> Unit
) {
	val intent = Intent(this, T::class.java)
	function(intent)
	startActivity(intent)
}

inline fun <reified T : Activity> Context.redirect() {
	redirect<T> {}
}

fun Activity.backWithExtras() {
	val extras = intent.extras ?: throw Exception("no extras")

	val parent = extras.get("__parent") as Class<*>
	extras.remove("__parent")

	val intent = Intent(this, parent)
	intent.putExtras(extras)

	startActivity(intent)
}

fun Activity.refresh() {
	finish()
	startActivity(intent)
}

internal fun BaseActivity.logout(api: Api) {
	api.logout(this::logoutLocal)
}

internal fun BaseActivity.logoutLocal() {
	clearAuth()
	redirect<LoginActivity>()
}

internal fun Activity.back() {
	finish()
}

internal fun Activity.close() {
	redirect<WelcomeActivity> {
		it.flags = Intent.FLAG_ACTIVITY_CLEAR_TOP
		it.putExtra("EXIT", true)
	}
}

internal fun Activity.goToSettings() {
	goToActivityWithBack(SettingsActivity::class.java)
}

internal fun Activity.createMove(extras: Bundle? = null) {
	goToActivityWithBack(MovesCreateActivity::class.java, extras)
}

private fun Activity.goToActivityWithBack(newActivity: Class<*>, extras: Bundle? = null) {
	val intent = Intent(this, newActivity)

	if (extras != null)
		intent.putExtras(extras)

	intent.putExtras(this.intent)
	intent.putExtra("__parent", javaClass)

	startActivity(intent)
}

fun Activity.composeEmail(subject: String, body: String, vararg addresses: String) {
	val intent = Intent(Intent.ACTION_SENDTO)

	// only email apps should handle this
	intent.data = Uri.parse("mailto:")

	intent.putExtra(Intent.EXTRA_EMAIL, addresses)
	intent.putExtra(Intent.EXTRA_SUBJECT, subject)
	intent.putExtra(Intent.EXTRA_TEXT, body)

	if (intent.resolveActivity(packageManager) != null) {
		startActivity(intent)
	}
}

fun Activity.composeErrorEmail(url: String, error: Throwable) {
	val subject = getString(R.string.error_mail_title)

	val body = url + "\n\n" +
		error.message + "\n\n" +
		error.stackTraceText

	val emails = getString(R.string.error_mail_address)

	composeEmail(subject, body, emails)
}
