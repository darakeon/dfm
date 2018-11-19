package com.darakeon.dfm.login

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.auth.setAuth
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.welcome.WelcomeActivity
import kotlinx.android.synthetic.main.login.email
import kotlinx.android.synthetic.main.login.password

class LoginActivity : BaseActivity() {
	override val contentView = R.layout.login
	override val title = R.string.login

	override val isLoggedIn: Boolean
		get() = false

	override val hasTitle: Boolean
		get() = false

	fun login(@Suppress(ON_CLICK) view: View) {
		api.login(
			email.text.toString(),
			password.text.toString()
		) {
			setAuth(it)
			redirect<WelcomeActivity>()
		}
	}
}
