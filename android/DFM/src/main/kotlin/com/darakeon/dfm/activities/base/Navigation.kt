package com.darakeon.dfm.activities.base

import android.app.Activity
import android.content.Intent
import android.os.Bundle
import com.darakeon.dfm.activities.LoginActivity
import com.darakeon.dfm.activities.MovesCreateActivity
import com.darakeon.dfm.activities.SettingsActivity
import com.darakeon.dfm.activities.WelcomeActivity
import com.darakeon.dfm.activities.objects.SmartStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.api.Step
import com.darakeon.dfm.user.ClearAuth
import com.darakeon.dfm.user.GetAuth

const val onClick: String = "UNUSED_PARAMETER"

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
	request.AddParameter("ticket", GetAuth())
	val tryResult = request.Post(Step.Logout)

	if (tryResult) {
		ClearAuth()
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

