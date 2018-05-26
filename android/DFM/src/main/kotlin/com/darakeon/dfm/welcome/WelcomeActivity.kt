package com.darakeon.dfm.welcome

import android.os.Bundle
import android.view.Window
import com.darakeon.dfm.R
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.base.SmartActivity
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.auth.isLoggedIn
import com.darakeon.dfm.login.LoginActivity

class WelcomeActivity : SmartActivity<WelcomeStatic>(WelcomeStatic) {

	override fun contentView(): Int = R.layout.welcome

	override val hasTitle: Boolean
		get() = false

	override fun onCreate(savedInstanceState: Bundle?) {
		this.requestWindowFeature(Window.FEATURE_NO_TITLE)

		super.onCreate(savedInstanceState)

		if (intent.getBooleanExtra("EXIT", false)) {
			finish()
			return
		}

		val request = InternalRequest(
			this, "", { startProgram() }
		)
		request.get()
	}

	private fun startProgram() {
		if (isLoggedIn())
			redirect<AccountsActivity>()
		else
			redirect<LoginActivity>()
	}

}
