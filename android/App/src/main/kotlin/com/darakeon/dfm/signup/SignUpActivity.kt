package com.darakeon.dfm.signup

import android.view.View
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.SignUpBinding
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.goToTerms
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.lib.extensions.toFormattedHour
import com.darakeon.dfm.welcome.WelcomeActivity
import java.util.Locale
import java.util.TimeZone

class SignUpActivity : BaseActivity<SignUpBinding>() {
	override fun inflateBinding(): SignUpBinding {
		return SignUpBinding.inflate(layoutInflater)
	}

	fun signUp(@Suppress(ON_CLICK) view: View) {
		val language = Locale.getDefault().toLanguageTag()
		val timezone = TimeZone.getDefault().toFormattedHour()

		callApi { api ->
			api.signup(
				binding.email.text.toString(),
				binding.password.text.toString(),
				binding.acceptTerms.isChecked,
				language,
				timezone,
			) {
				api.login(
					binding.email.text.toString(),
					binding.password.text.toString()
				) {
					ticket = it.ticket
					redirect<WelcomeActivity>()
				}
			}
		}
	}

	fun goToTerms(@Suppress(ON_CLICK) view: View) {
		goToTerms()
	}
}
