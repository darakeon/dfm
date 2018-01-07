package com.darakeon.dfm.activities

import android.os.Bundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.*
import com.darakeon.dfm.activities.objects.LoginStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.user.setAuth
import org.json.JSONObject

class LoginActivity : SmartActivity<LoginStatic>(LoginStatic) {
	override fun contentView(): Int = R.layout.login

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (!isProd) {
			setValue(R.id.email, getString(R.string.local_email))
			setValue(R.id.password, getString(R.string.local_password))
		}
	}

	override val isLoggedIn: Boolean
		get() = false

	override val hasTitle: Boolean
		get() = false


	fun login(@Suppress(ON_CLICK) view: View) {
		val request = InternalRequest(
			this, "Users/Login", { d -> handleLogin(d) }
		)

		request.addParameter("email", getValue(R.id.email))
		request.addParameter("password", getValue(R.id.password))

		request.post()
	}

	private fun handleLogin(data: JSONObject) {
		val ticket = data.getString("ticket")
		setAuth(ticket)
		redirect(WelcomeActivity::class.java)
	}


}
