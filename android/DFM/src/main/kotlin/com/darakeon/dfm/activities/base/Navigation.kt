package com.darakeon.dfm.activities.base

import android.app.Activity
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import com.darakeon.dfm.activities.LoginActivity
import com.darakeon.dfm.activities.MovesCreateActivity
import com.darakeon.dfm.activities.SettingsActivity
import com.darakeon.dfm.activities.WelcomeActivity
import com.darakeon.dfm.activities.objects.SmartStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.user.clearAuth
import com.darakeon.dfm.user.getAuth

const val ON_CLICK: String = "UNUSED_PARAMETER"

fun Activity.redirect(activityClass: Class<*>) {
	val intent = Intent(this, activityClass)
	startActivity(intent)
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
		redirect(LoginActivity::class.java)
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
