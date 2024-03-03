package com.darakeon.dfm.error_logs

import android.os.Bundle
import android.view.View
import com.darakeon.dfm.error_logs.databinding.StartBinding
import com.darakeon.dfm.error_logs.service.SiteErrorService
import com.darakeon.dfm.lib.api.entities.login.Login
import com.darakeon.dfm.lib.extensions.redirect

class StartActivity : BaseActivity<StartBinding>() {
	override fun inflateBinding(): StartBinding {
		return StartBinding.inflate(layoutInflater)
	}

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (auth.isLoggedIn) {
			startServiceAndGoToList()
		}
	}

	private fun startServiceAndGoToList() {
		SiteErrorService.start(this)
		redirect<ListActivity>()
		finish()
	}

	@Suppress("UNUSED_PARAMETER")
	fun login(view: View) {
		if (auth.isLoggedIn) {
			proceedLogin(Login(auth.ticket))
		} else {
			api.login(
				binding.email.text.toString(),
				binding.password.text.toString(),
				this::proceedLogin
			)
		}
	}

	private fun proceedLogin(it: Login) {
		auth.ticket = it.ticket
		api.validateTFA(
			binding.tfa.text.toString(),
			this::startServiceAndGoToList
		)
	}
}
