package com.darakeon.dfm.activities.base

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

class Navigation<T : SmartStatic> internal constructor(private val activity: SmartActivity<T>) {


	fun redirect(activityClass: Class<*>) {
		val intent = Intent(activity, activityClass)
		activity.startActivity(intent)
	}

	fun redirectWithExtras() {
		val extras = activity.intent.extras
		val parent = extras.get("__parent") as Class<*>
		extras.remove("__parent")

		val intent = Intent(activity, parent)
		intent.putExtras(extras)

		activity.startActivity(intent)
	}


	internal fun logout() {
		val request = InternalRequest(activity, "Users/Logout")
		request.AddParameter("ticket", activity.GetAuth())
		val tryResult = request.Post(Step.Logout)

		if (tryResult) {
			activity.ClearAuth()
			redirect(LoginActivity::class.java)
		}
	}

	internal fun back() {
		activity.finish()
	}

	internal fun close() {
		val intent = Intent(activity, WelcomeActivity::class.java)
		intent.flags = Intent.FLAG_ACTIVITY_CLEAR_TOP
		intent.putExtra("EXIT", true)
		activity.startActivity(intent)
	}

	internal fun goToSettings() {
		goToActivityWithBack(SettingsActivity::class.java)
	}

	internal fun createMove(extras: Bundle? = null) {
		goToActivityWithBack(MovesCreateActivity::class.java, extras)
	}

	private fun goToActivityWithBack(newActivity: Class<*>, extras: Bundle? = null) {
		val intent = Intent(activity, newActivity)

		if (extras != null)
			intent.putExtras(extras)

		intent.putExtras(activity.intent)
		intent.putExtra("__parent", activity.javaClass)

		activity.startActivity(intent)
	}


}
