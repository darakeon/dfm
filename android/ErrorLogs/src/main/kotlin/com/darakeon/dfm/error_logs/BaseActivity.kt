package com.darakeon.dfm.error_logs

import android.app.Activity
import android.os.Bundle
import android.view.Menu
import android.view.MenuInflater
import android.widget.Toast
import androidx.viewbinding.ViewBinding
import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.api.ApiCaller
import com.darakeon.dfm.lib.api.entities.Environment
import com.darakeon.dfm.lib.api.entities.Theme
import com.darakeon.dfm.lib.auth.Authentication
import com.darakeon.dfm.lib.auth.setEnvironment

open class BaseActivity<Binding: ViewBinding>: Activity(), ApiCaller {
	protected lateinit var auth: Authentication
	protected lateinit var api: Api<BaseActivity<Binding>>
	private var serverUrl: String? = null

	protected lateinit var binding: Binding
	protected open fun inflateBinding(): Binding {
		throw NotImplementedError()
	}

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)
		setEnvironment(Environment(Theme.DarkMagic))

		api = Api(this, serverUrl)
		auth = Authentication(this)

		binding = inflateBinding()
		setContentView(binding.root)
 	}

	override fun onCreateOptionsMenu(menu: Menu): Boolean {
		val inflater: MenuInflater = menuInflater
		inflater.inflate(R.menu.pig, menu)
		return true
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

	override fun error(resMessage: Int, resButton: Int, action: () -> Unit) {
		error(getString(resMessage))
		action()
	}

	override fun error(url: String, error: Throwable) {
		error("$url: ${error.message}")
	}
}
