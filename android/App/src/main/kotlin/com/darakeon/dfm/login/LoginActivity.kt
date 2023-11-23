package com.darakeon.dfm.login

import android.content.Intent
import android.net.Uri
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.LoginBinding
import com.darakeon.dfm.dialogs.confirm
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.goToTerms
import com.darakeon.dfm.lib.api.MainInfo
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.welcome.WelcomeActivity

class LoginActivity : BaseActivity<LoginBinding>() {
	override fun inflateBinding(): LoginBinding {
		return LoginBinding.inflate(layoutInflater)
	}

	fun login(@Suppress(ON_CLICK) view: View) {
		callApi { api ->
			api.login(
				binding.email.text.toString(),
				binding.password.text.toString()
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

	fun goToTerms(@Suppress(ON_CLICK) view: View) {
		goToTerms()
	}
}
