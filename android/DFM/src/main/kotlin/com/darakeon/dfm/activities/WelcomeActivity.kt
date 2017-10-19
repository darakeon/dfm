package com.darakeon.dfm.activities

import android.os.Bundle
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.base.redirect
import com.darakeon.dfm.activities.objects.WelcomeStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.user.IsLoggedIn
import org.json.JSONObject
import kotlin.reflect.KClass

class WelcomeActivity : SmartActivity<WelcomeStatic>(WelcomeStatic) {

	override fun contentView(): Int = R.layout.welcome

	override val hasTitle: Boolean
		get() = false

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (intent.getBooleanExtra("EXIT", false)) {
			finish()
			return
		}

		val request = InternalRequest(
			this, "", { d -> startProgram(d) }
		)
		request.Get()
	}

	private fun startProgram(data: JSONObject) {
		val nextActivity : KClass<*> =
			if (IsLoggedIn())
				AccountsActivity::class
			else
				LoginActivity::class

		redirect(nextActivity.java)
	}

}
