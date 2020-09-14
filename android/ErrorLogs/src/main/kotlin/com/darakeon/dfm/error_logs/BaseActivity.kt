package com.darakeon.dfm.error_logs

import android.app.Activity
import android.os.Bundle
import android.widget.Toast
import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.api.ApiCaller
import com.darakeon.dfm.lib.auth.Authentication

open class BaseActivity: Activity(), ApiCaller {
	protected lateinit var auth: Authentication
	override lateinit var api: Api<BaseActivity>
	private var serverUrl: String? = null

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)
		auth = Authentication(this)
		api = Api(this, serverUrl)
	}

	override val ticket: String
		get() = auth.ticket

	override fun logout() {
		auth.clear()
	}

	override fun checkTFA() {
		auth.clear()
	}

	override fun error(text: String) {
		Toast.makeText(
			this, text, Toast.LENGTH_LONG
		).show()
	}

	override fun error(resId: Int) {
		error(getString(resId))
	}

	override fun error(resId: Int, action: () -> Unit) {
		error(getString(resId))
		action()
	}

	override fun error(url: String, error: Throwable) {
		error("$url: ${error.message}")
	}
}
