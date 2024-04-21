package com.darakeon.dfm.base

import android.app.Activity
import android.os.Bundle
import android.view.Menu
import android.view.MenuInflater
import android.view.View
import android.view.Window
import android.widget.Button
import android.widget.Toast
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import androidx.viewbinding.ViewBinding
import com.darakeon.dfm.R
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.databinding.BottomMenuBinding
import com.darakeon.dfm.dialogs.alertError
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.back
import com.darakeon.dfm.extensions.createMove
import com.darakeon.dfm.extensions.goToSettings
import com.darakeon.dfm.extensions.logout
import com.darakeon.dfm.extensions.logoutLocal
import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.api.ApiCaller
import com.darakeon.dfm.lib.api.entities.AccountComboItem
import com.darakeon.dfm.lib.api.entities.ComboItem
import com.darakeon.dfm.lib.auth.Authentication
import com.darakeon.dfm.lib.auth.getValue
import com.darakeon.dfm.lib.auth.getValueTyped
import com.darakeon.dfm.lib.auth.recoverEnvironment
import com.darakeon.dfm.lib.auth.setValue
import com.darakeon.dfm.lib.auth.setValueTyped
import com.darakeon.dfm.lib.extensions.applyGlyphicon
import com.darakeon.dfm.lib.extensions.composeErrorApi
import com.darakeon.dfm.lib.extensions.composeErrorEmail
import com.darakeon.dfm.lib.extensions.contact
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.lib.extensions.refresh
import com.darakeon.dfm.moves.MovesActivity
import com.darakeon.dfm.settings.SettingsActivity
import com.darakeon.dfm.tfa.TFAActivity

abstract class BaseActivity<Binding: ViewBinding>: Activity(), ApiCaller {
	private var api: Api<BaseActivity<Binding>>? = null
	private var serverUrl: String? = null

	protected fun callApi(call: (Api<BaseActivity<Binding>>) -> Unit) {
		val api = api

		if (api == null) {
			alertError(R.string.error_call_api, R.string.send_report_button) {
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

	protected open val contentViewId: Int = 0
	protected lateinit var binding: Binding
	protected open fun inflateBinding(): Binding {
		throw NotImplementedError()
	}
	protected open fun getMenuBinding(): BottomMenuBinding? {
		return null
	}
	protected open fun getLogoutButton(): Button? {
		return getMenuBinding()?.actionLogout
	}

	protected open val title: Int = 0

	protected open val refresh: SwipeRefreshLayout? = null

	protected val isLoggedIn
		get() = auth?.isLoggedIn ?: false

	private val hasTitle
		get() = title != 0

	val query = HashMap<String, String>()

	private var cachedCombos: Boolean
		get() = this.getValue("cachedCombos").toBoolean()
		set(value) = this.setValue("cachedCombos", value.toString())

	protected var accountCombo: Array<AccountComboItem>
		get() = this.getValueTyped("accountCombo") ?: emptyArray()
		private set(value) = this.setValueTyped("accountCombo", value)

	protected var categoryCombo: Array<ComboItem>
		get() = this.getValueTyped("categoryCombo") ?: emptyArray()
		private set(value) = this.setValueTyped("categoryCombo", value)

	protected var isUsingCategories: Boolean
		get() = this.getValueTyped("isUsingCategories") ?: false
		private set(value) = this.setValueTyped("isUsingCategories", value)

	protected fun populateCache(
		refresh: Boolean = false,
		execute: (() -> Unit)? = null
	) {
		if (cachedCombos && !refresh) return

		callApi { api ->
			api.listsForMoves {
				isUsingCategories = it.isUsingCategories
				accountCombo = it.accountList
				categoryCombo = it.categoryList
				cachedCombos = true

				if (execute != null) execute()
			}
		}
	}

	@Suppress(ON_CLICK)
	fun updateCache(view: View) {
		populateCache(true, this::refresh)
	}

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

		if (contentViewId != 0) {
			setContentView(contentViewId)
		} else {
			binding = inflateBinding()
			setContentView(binding.root)
		}
	}

	private fun setMenuLongClicks() {
		val logout = getLogoutButton() ?: return
		logout.setOnLongClickListener {
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
		val menuBinding = getMenuBinding() ?: return

		(0 until menuBinding.bottomMenu.childCount).forEach {
			val button = menuBinding.bottomMenu.getChildAt(it) as Button
			button.applyGlyphicon()

			if (this is AccountsActivity)
				menuBinding.actionHome.isEnabled = false

			if (this is SettingsActivity)
				menuBinding.actionSettings.isEnabled = false

			if (this is MovesActivity)
				menuBinding.actionMove.isEnabled = false
		}
	}

	override fun onCreateOptionsMenu(menu: Menu): Boolean {
		val inflater: MenuInflater = menuInflater
		inflater.inflate(R.menu.pig, menu)
		return true
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
	override fun error(resMessage: Int, resButton: Int, action: () -> Unit) = alertError(resMessage, resButton, action)
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
