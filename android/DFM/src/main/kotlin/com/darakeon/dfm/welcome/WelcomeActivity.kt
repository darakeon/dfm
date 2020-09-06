package com.darakeon.dfm.welcome

import android.content.Intent
import android.os.Bundle
import com.darakeon.dfm.R
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.login.LoginActivity
import com.darakeon.dfm.service.SiteErrorService

class WelcomeActivity : BaseActivity() {
	override val contentView = R.layout.welcome

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (intent.getBooleanExtra("EXIT", false)) {
			finish()
			return
		}

		callApi {
			it.wakeUpSite {
				startServices()
				redirect()
			}
		}
	}

	private fun startServices() {
		if (isAdm) {
			val intent = Intent(this, SiteErrorService::class.java)
			startService(intent)
		}
	}

	private fun redirect() {
		if (isLoggedIn)
			redirect<AccountsActivity>()
		else
			redirect<LoginActivity>()
	}
}
