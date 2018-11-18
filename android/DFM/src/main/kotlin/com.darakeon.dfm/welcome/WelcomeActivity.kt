package com.darakeon.dfm.welcome

import android.os.Bundle
import android.view.Window
import com.darakeon.dfm.R
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.login.LoginActivity

class WelcomeActivity : BaseActivity<WelcomeStatic>(WelcomeStatic) {

	override val contentView = R.layout.welcome
	override val hasTitle = false

	override fun onCreate(savedInstanceState: Bundle?) {
		this.requestWindowFeature(Window.FEATURE_NO_TITLE)

		super.onCreate(savedInstanceState)

		if (intent.getBooleanExtra("EXIT", false)) {
			finish()
			return
		}

		api.wakeupSite {
			if (isLoggedIn)
				redirect<AccountsActivity>()
			else
				redirect<LoginActivity>()
		}
	}
}
