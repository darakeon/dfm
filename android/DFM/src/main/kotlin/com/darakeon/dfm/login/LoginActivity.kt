package com.darakeon.dfm.login

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.welcome.WelcomeActivity
import kotlinx.android.synthetic.main.login.email
import kotlinx.android.synthetic.main.login.password

class LoginActivity : BaseActivity() {
	override val contentView = R.layout.login

	fun login(@Suppress(ON_CLICK) view: View) {
		callApi { api ->
			api.login(
				email.text.toString(),
				password.text.toString()
			) {
				ticket = it.ticket
				isAdm = it.isAdm
				redirect<WelcomeActivity>()
			}
		}
	}
}
