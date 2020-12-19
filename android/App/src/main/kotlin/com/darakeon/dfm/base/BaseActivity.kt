package com.darakeon.dfm.base

import android.app.Activity
import android.os.Bundle
import android.view.View
import android.view.Window
import android.widget.Button
import android.widget.Toast
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.dialogs.alertError
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.back
import com.darakeon.dfm.extensions.composeErrorApi
import com.darakeon.dfm.extensions.composeErrorEmail
import com.darakeon.dfm.extensions.contact
import com.darakeon.dfm.extensions.createMove
import com.darakeon.dfm.extensions.goToSettings
import com.darakeon.dfm.extensions.logout
import com.darakeon.dfm.extensions.logoutLocal
import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.api.ApiCaller
import com.darakeon.dfm.lib.auth.Authentication
import com.darakeon.dfm.lib.auth.recoverEnvironment
import com.darakeon.dfm.lib.extensions.applyGlyphicon
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.lib.extensions.refresh
import com.darakeon.dfm.moves.MovesActivity
import com.darakeon.dfm.settings.SettingsActivity
import com.darakeon.dfm.tfa.TFAActivity
import kotlinx.android.synthetic.main.bottom_menu.action_home
import kotlinx.android.synthetic.main.bottom_menu.action_logout
import kotlinx.android.synthetic.main.bottom_menu.action_move
import kotlinx.android.synthetic.main.bottom_menu.action_settings
import kotlinx.android.synthetic.main.bottom_menu.bottom_menu
import java.util.HashMap
import kotlinx.android.synthetic.main.welcome.action_logout as welcome_logout

abstract class BaseActivity: Activity(), ApiCaller {
	override var api: Api<BaseActivity>? = null
	private var serverUrl: String? = null

	protected fun callApi(call: (Api<BaseActivity>) -> Unit) {
		val api = api

		if (api == null) {
			alertError(R.string.error_call_api) {
				composeErrorApi()
			}
		} else {
			call(api)
		}
	}

	private var auth: Authentication? = null
	private var ui: UIHandler? = null

	override var ticket: String
		get() = auth?.ticket ?: ""
		set(value) { auth?.ticket = value }

	open fun clearAuth() = auth?.clear()

	protected abstract val contentView: Int
	protected open val title: Int = 0

	protected open val refresh: SwipeRefreshLayout? = null

	protected val isLoggedIn
		get() = auth?.isLoggedIn ?: false

	private val hasTitle
		get() = title != 0

	val query = HashMap<String, String>()

	override fun onCreate(savedInstanceState: Bundle?) {
		recoverEnvironment()

		super.onCreate(savedInstanceState)

		api = Api(this, serverUrl)
		auth = Authentication(this)
		ui = UIHandler(this)

		handleScreen()
		setMenuLongClicks()
		processQuery()
		customizeBottomMenu()

		// TODO: test it
		refresh?.setOnRefreshListener { refresh() }
	}

	private fun handleScreen() {
		if (hasTitle)
			setTitle(title)
		else
			requestWindowFeature(Window.FEATURE_NO_TITLE)

		if (contentView != 0)
			setContentView(contentView)
	}

	private fun setMenuLongClicks() {
		val logout = action_logout ?: welcome_logout
		logout?.setOnLongClickListener {
			logout(api)
			true
		}
	}

	private fun processQuery() {
		val data = intent.data
		if (data?.queryParameterNames != null) {
			for (param: String in data.queryParameterNames) {
				query[param] = data.getQueryParameter(param) ?: ""
			}
		}
	}

	private fun customizeBottomMenu() {
		if (bottom_menu == null) return

		(0 until bottom_menu.childCount).forEach {
			val button = bottom_menu.getChildAt(it) as Button
			button.applyGlyphicon()

			if (this is AccountsActivity)
				action_home.isEnabled = false

			if (this is SettingsActivity)
				action_settings.isEnabled = false

			if (this is MovesActivity)
				action_move.isEnabled = false
		}
	}

	override fun onDestroy() {
		super.onDestroy()
		api?.cancel()
	}

	fun back(@Suppress(ON_CLICK) view: View) {
		back()
	}

	fun refresh(@Suppress(ON_CLICK) view: View) {
		refresh()
	}

	fun contact(@Suppress(ON_CLICK) view: View) {
		contact()
	}

	fun showLongClickWarning(@Suppress(ON_CLICK) view: View) {
		Toast.makeText(
			this,
			R.string.long_click_warning,
			Toast.LENGTH_LONG
		).show()
	}

	fun goToAccounts(@Suppress(ON_CLICK) view: View) {
		redirect<AccountsActivity>()
	}

	fun goToSettings(@Suppress(ON_CLICK) view: View) {
		goToSettings()
	}

	fun createMove(@Suppress(ON_CLICK) view: View) {
		createMove()
	}

	protected fun getExtraOrUrl(key: String) : String? {
		val extras = intent?.extras

		if (extras?.containsKey(key) == true) {
			return extras[key]?.toString()
		}

		if (query.containsKey(key)) {
			return query[key]
		}

		return null
	}

	override fun error(url: String, error: Throwable) = composeErrorEmail(url, error)
	override fun error(resId: Int) = alertError(resId)
	override fun error(resId: Int, action: () -> Unit) = alertError(resId, action)
	override fun error(text: String) = alertError(text)
	override fun logout() = logoutLocal()

	override fun checkTFA() {
		redirect<TFAActivity>()
	}

	override fun startWait() {
		ui?.startUIWait()
	}

	override fun endWait() {
		ui?.endUIWait()
	}
}
