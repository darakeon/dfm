package com.darakeon.dfm.activities

import android.os.Bundle
import android.view.Window
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.base.redirect
import com.darakeon.dfm.activities.objects.WelcomeStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.user.isLoggedIn

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
