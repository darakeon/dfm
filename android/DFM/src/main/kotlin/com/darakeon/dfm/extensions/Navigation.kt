package com.darakeon.dfm.extensions

import android.app.Activity
import android.content.Context
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import com.darakeon.dfm.base.SmartActivity
import com.darakeon.dfm.base.SmartStatic
import com.darakeon.dfm.login.LoginActivity
import com.darakeon.dfm.moves.MovesCreateActivity
import com.darakeon.dfm.settings.SettingsActivity
import com.darakeon.dfm.welcome.WelcomeActivity
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.auth.clearAuth
import com.darakeon.dfm.auth.getAuth

const val ON_CLICK: String = "UNUSED_PARAMETER"

inline fun <reified T : Activity> Context.redirect () {
	val intent = Intent(this, T::class.java)
	startActivity(intent)
}

inline fun <reified T : Activity> Activity.redirectAndFinish () {
	redirect<T>()
	finish()
}

fun Activity.redirectWithExtras() {
	val extras = intent.extras
	val parent = extras.get("__parent") as Class<*>
	extras.remove("__parent")

	val intent = Intent(this, parent)
	intent.putExtras(extras)

	startActivity(intent)
}


internal fun <T : SmartStatic> SmartActivity<T>.logout() {
	val request = InternalRequest(this, "Users/Logout")
	request.addParameter("ticket", getAuth())
	val tryResult = request.post()

	if (tryResult) {
		clearAuth()
		redirect<LoginActivity>()
	}
}

internal fun Activity.back() {
	finish()
}

internal fun Activity.close() {
	val intent = Intent(this, WelcomeActivity::class.java)
	intent.flags = Intent.FLAG_ACTIVITY_CLEAR_TOP
	intent.putExtra("EXIT", true)
	startActivity(intent)
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
