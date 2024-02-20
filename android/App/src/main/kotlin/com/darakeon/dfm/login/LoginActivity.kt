package com.darakeon.dfm.login

import android.view.View
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.LoginBinding
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.goToTerms
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.signup.SignUpActivity
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
		redirect<SignUpActivity>()
	}

	fun goToTerms(@Suppress(ON_CLICK) view: View) {
		goToTerms()
	}
}
