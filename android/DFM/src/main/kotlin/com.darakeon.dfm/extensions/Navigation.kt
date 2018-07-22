package com.darakeon.dfm.extensions

import android.app.Activity
import android.content.Context
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import com.darakeon.dfm.R
import com.darakeon.dfm.api.old.InternalRequest
import com.darakeon.dfm.auth.clearAuth
import com.darakeon.dfm.auth.getAuth
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.base.SmartStatic
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
	redirect<T>({})
}

inline fun <reified T : Activity> Activity.redirectAndFinish() {
	redirect<T>()
	finish()
}

fun Activity.backWithExtras() {
	val extras = intent.extras
	val parent = extras.get("__parent") as Class<*>
	extras.remove("__parent")

	val intent = Intent(this, parent)
	intent.putExtras(extras)

	startActivity(intent)
}

internal fun <T : SmartStatic> BaseActivity<T>.logout() {
	val request = InternalRequest(this, "Users/Logout")
	request.addParameter("ticket", getAuth())
	val tryResult = request.post()

	if (tryResult) {
		logoutLocal()
	}
}

internal fun Activity.logoutLocal() {
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

	val reportUrl =
		Regex("Api-\\w{32}")
			.replace(url, "Api-{ticket}")

	val body = reportUrl + "\n\n" +
		error.message + "\n\n" +
		error.stackTrace

	val emails = getString(R.string.error_mail_address)

	composeEmail(subject, body, emails)
}
