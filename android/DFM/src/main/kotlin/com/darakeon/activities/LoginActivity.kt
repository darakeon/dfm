package com.dontflymoney.view

import android.view.View
import com.dontflymoney.activityObjects.LoginStatic
import com.dontflymoney.api.InternalRequest
import com.dontflymoney.api.Step
import com.dontflymoney.baseactivity.SmartActivity
import org.json.JSONException
import org.json.JSONObject


class LoginActivity : SmartActivity<LoginStatic>(LoginStatic) {
    override fun contentView(): Int {
        return R.layout.login
    }

    override val isLoggedIn: Boolean
        get() = false


    fun login(view: View) {
        val request = InternalRequest(this, "Users/Login")

        request.AddParameter("email", form.getValue(R.id.email))
        request.AddParameter("password", form.getValue(R.id.password))

        request.Post()
    }

    @Throws(JSONException::class)
    override fun HandleSuccess(data: JSONObject, step: Step) {
        val ticket = data.getString("ticket")
        Authentication.Set(ticket)
        navigation.redirect(AccountsActivity::class.java)
    }


}
