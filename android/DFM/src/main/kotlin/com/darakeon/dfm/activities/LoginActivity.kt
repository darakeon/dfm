package com.darakeon.dfm.activities

import android.view.View
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.objects.LoginStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.api.Step
import com.darakeon.dfm.R
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
