package com.darakeon.dfm.activities

import android.os.Bundle
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.objects.WelcomeStatic
import com.darakeon.dfm.api.Step
import org.json.JSONObject
import kotlin.reflect.KClass

class WelcomeActivity : SmartActivity<WelcomeStatic>(WelcomeStatic) {

	override fun HandleSuccess(data: JSONObject, step: Step) {
	}

	override fun contentView(): Int = R.layout.welcome

	override val hasTitle: Boolean
		get() = false


	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (intent.getBooleanExtra("EXIT", false)) {
			finish()
			return
		}

		val nextActivity : KClass<*> =
				if (Authentication.IsLoggedIn())
					AccountsActivity::class
				else
					LoginActivity::class

		navigation.redirect(nextActivity.java)
	}
}
