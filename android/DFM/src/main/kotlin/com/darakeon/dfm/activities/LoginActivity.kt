package com.darakeon.dfm.activities

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.base.getValue
import com.darakeon.dfm.activities.base.onClick
import com.darakeon.dfm.activities.objects.LoginStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.api.Step
import org.json.JSONObject

class LoginActivity : SmartActivity<LoginStatic>(LoginStatic) {
	override fun contentView(): Int {
		return R.layout.login
	}

	override val isLoggedIn: Boolean
		get() = false

	override val hasTitle: Boolean
		get() = false


	fun login(@Suppress(onClick) view: View) {
		val request = InternalRequest(this, "Users/Login")

		request.AddParameter("email", getValue(R.id.email))
		request.AddParameter("password", getValue(R.id.password))

		request.Post()
	}

	override fun HandleSuccess(data: JSONObject, step: Step) {
		val ticket = data.getString("ticket")
		Authentication.Set(ticket)
		navigation.redirect(WelcomeActivity::class.java)
	}


}
