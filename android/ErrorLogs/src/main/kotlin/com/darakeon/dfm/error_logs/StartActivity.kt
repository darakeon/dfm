package com.darakeon.dfm.error_logs

import android.content.Intent
import android.os.Bundle
import android.view.View
import com.darakeon.dfm.error_logs.service.SiteErrorService
import com.darakeon.dfm.lib.api.entities.login.Login
import kotlinx.android.synthetic.main.activity_start.email
import kotlinx.android.synthetic.main.activity_start.password
import kotlinx.android.synthetic.main.activity_start.tfa

class StartActivity : BaseActivity() {
	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)
		setContentView(R.layout.activity_start)

		if (auth.isLoggedIn) {
			startServiceAndGoToList()
		}
	}

	private fun startServiceAndGoToList() {
		if (!SiteErrorService.running) {
			startService(Intent(this, SiteErrorService::class.java))
		}

		startActivity(Intent(this, ListActivity::class.java))
		finish()
	}

	@Suppress("UNUSED_PARAMETER")
	fun login(view: View) {
		if (auth.isLoggedIn) {
			proceedLogin(Login(auth.ticket))
		} else {
			api.login(
				email.text.toString(),
				password.text.toString(),
				this::proceedLogin
			)
		}
	}

	private fun proceedLogin(it: Login) {
		auth.ticket = it.ticket
		api.validateTFA(
			tfa.text.toString(),
			this::startServiceAndGoToList
		)
	}
}
