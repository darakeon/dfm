package com.dontflymoney.baseactivity

import android.content.Intent
import android.os.Bundle
import com.dontflymoney.activityObjects.SmartStatic

import com.dontflymoney.api.InternalRequest
import com.dontflymoney.api.Step
import com.dontflymoney.userdata.Authentication
import com.dontflymoney.view.LoginActivity
import com.dontflymoney.view.SettingsActivity

class Navigation<T : SmartStatic> internal constructor(private val activity: SmartActivity<T>, private val authentication: Authentication) {


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
        request.AddParameter("ticket", authentication.Get()!!)
        val tryResult = request.Post(Step.Logout)

        if (tryResult) {
            authentication.Clear()
            redirect(LoginActivity::class.java)
        }
    }

    internal fun back() {
        activity.finish()
    }

    internal fun close() {
        val intent = Intent(activity, LoginActivity::class.java)
        intent.flags = Intent.FLAG_ACTIVITY_CLEAR_TOP
        intent.putExtra("EXIT", true)
        activity.startActivity(intent)
    }

    internal fun goToSettings() {
        val intent = Intent(activity, SettingsActivity::class.java)

        intent.putExtras(activity.intent)
        intent.putExtra("__parent", activity.javaClass)

        activity.startActivity(intent)
    }


}
