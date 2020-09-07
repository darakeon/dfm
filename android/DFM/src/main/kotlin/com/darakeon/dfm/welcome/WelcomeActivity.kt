package com.darakeon.dfm.welcome

import android.os.Bundle
import com.darakeon.dfm.R
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.login.LoginActivity

class WelcomeActivity : BaseActivity() {
	override val contentView = R.layout.welcome

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (intent.getBooleanExtra("EXIT", false)) {
			finish()
			return
		}

		callApi { it.wakeUpSite { redirect() } }
	}

	private fun redirect() {
		if (isLoggedIn)
			redirect<AccountsActivity>()
		else
			redirect<LoginActivity>()
	}
}
