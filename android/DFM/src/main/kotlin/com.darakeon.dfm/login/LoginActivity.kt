package com.darakeon.dfm.login

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.login.Login
import com.darakeon.dfm.api.old.DELETE
import com.darakeon.dfm.auth.setAuth
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.welcome.WelcomeActivity
import kotlinx.android.synthetic.main.login.email
import kotlinx.android.synthetic.main.login.password

class LoginActivity : BaseActivity<DELETE>(DELETE) {
	override val contentView = R.layout.login

	override val isLoggedIn: Boolean
		get() = false

	override val hasTitle: Boolean
		get() = false

	fun login(@Suppress(ON_CLICK) view: View) {
		val login = Login.Request(
			email.text.toString(),
			password.text.toString()
		)

		api.login(login) {
			setAuth(it.ticket)
			redirect<WelcomeActivity>()
		}
	}
}
