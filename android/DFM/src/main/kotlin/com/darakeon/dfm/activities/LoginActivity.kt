package com.darakeon.dfm.activities

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.ON_CLICK
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.base.redirect
import com.darakeon.dfm.activities.objects.LoginStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.user.setAuth
import kotlinx.android.synthetic.main.login.*
import org.json.JSONObject

class LoginActivity : SmartActivity<LoginStatic>(LoginStatic) {
	override fun contentView(): Int = R.layout.login

	override val isLoggedIn: Boolean
		get() = false

	override val hasTitle: Boolean
		get() = false


	fun login(@Suppress(ON_CLICK) view: View) {
		val request = InternalRequest(
			this, "Users/Login", { d -> handleLogin(d) }
		)

		request.addParameter("email", email.text)
		request.addParameter("password", password.text)

		request.post()
	}

	private fun handleLogin(data: JSONObject) {
		val ticket = data.getString("ticket")
		setAuth(ticket)
		redirect<WelcomeActivity>()
	}


}
