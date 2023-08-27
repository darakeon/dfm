package com.darakeon.dfm.login

import android.content.Intent
import android.net.Uri
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.LoginBinding
import com.darakeon.dfm.dialogs.confirm
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.lib.api.MainInfo
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.welcome.WelcomeActivity
import kotlinx.android.synthetic.main.login.email
import kotlinx.android.synthetic.main.login.password

class LoginActivity : BaseActivity<LoginBinding>() {
	override val contentViewId = R.layout.login

	fun login(@Suppress(ON_CLICK) view: View) {
		callApi { api ->
			api.login(
				email.text.toString(),
				password.text.toString()
			) {
				ticket = it.ticket
				redirect<WelcomeActivity>()
			}
		}
	}

	fun signUp(@Suppress(ON_CLICK) view: View) {
		confirm(R.string.go_to_sign_up) {
			val url = "${MainInfo.getSiteUrl(this)}Users/SignUp"
			val intent = Intent(Intent.ACTION_VIEW)
			intent.data = Uri.parse(url)
			startActivity(intent)
		}
	}
}
