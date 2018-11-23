package com.darakeon.dfm.base

import android.app.Activity
import android.content.Context
import android.os.Bundle
import android.view.ContextMenu
import android.view.LayoutInflater
import android.view.Menu
import android.view.View
import android.view.Window
import android.widget.Button
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.api.Api
import com.darakeon.dfm.auth.Authentication
import com.darakeon.dfm.auth.languageChangeFromSaved
import com.darakeon.dfm.auth.themeChangeFromSaved
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.applyGlyphicon
import com.darakeon.dfm.extensions.back
import com.darakeon.dfm.extensions.close
import com.darakeon.dfm.extensions.createMove
import com.darakeon.dfm.extensions.goToSettings
import com.darakeon.dfm.extensions.logout
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.extensions.refresh
import com.darakeon.dfm.moves.MovesCreateActivity
import com.darakeon.dfm.settings.SettingsActivity
import kotlinx.android.synthetic.main.bottom_menu.action_home
import kotlinx.android.synthetic.main.bottom_menu.action_move
import kotlinx.android.synthetic.main.bottom_menu.action_settings
import kotlinx.android.synthetic.main.bottom_menu.bottom_menu
import java.util.HashMap

abstract class BaseActivity : Activity() {

	var clickedView: View? = null
	private lateinit var inflater: LayoutInflater

	protected lateinit var api: Api
	private lateinit var auth: Authentication

	var ticket
		get() = auth.ticket
		set(value) { auth.ticket = value }

	fun clearAuth() = auth.clear()

	protected abstract val contentView: Int
	protected open val title: Int = 0
	protected open val optionsMenuResource = 0
	protected open val contextMenuResource = 0
	protected open val viewWithContext: View? = null
	protected open val highlight: View? = null
	protected open val isLoggedIn = true
	protected open val hasTitle = true

	protected open fun changeContextMenu(view: View, menuInfo: ContextMenu) {}

	val query = HashMap<String, String>()

	override fun onCreate(savedInstanceState: Bundle?) {
		languageChangeFromSaved()
		themeChangeFromSaved()

		super.onCreate(savedInstanceState)

		api = Api(this)
		auth = Authentication(this)

		if (!hasTitle)
			requestWindowFeature(Window.FEATURE_NO_TITLE)

		inflater = getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

		if (hasTitle)
			setTitle(title)

		setContentView(contentView)

		if (viewWithContext != null) {
			registerForContextMenu(viewWithContext)
		}

		val data = intent.data
		if (data?.queryParameterNames != null) {
			for (param: String in data.queryParameterNames) {
				query[param] = data.getQueryParameter(param) ?: ""
			}
		}
	}

	override fun onCreateOptionsMenu(menu: Menu): Boolean {
		super.onCreateOptionsMenu(menu)

		if (optionsMenuResource != 0)
			menuInflater.inflate(optionsMenuResource, menu)

		if (bottom_menu != null)
		{
			(0 until bottom_menu.childCount).forEach {
				val button = bottom_menu.getChildAt(it) as Button
				button.applyGlyphicon(this)

				if (this is AccountsActivity)
					action_home.isEnabled = false

				if (this is SettingsActivity)
					action_settings.isEnabled = false

				if (this is MovesCreateActivity)
					action_move.isEnabled = false
			}
		}

		return true
	}

	override fun onCreateContextMenu(menu: ContextMenu, v: View, menuInfo: ContextMenu.ContextMenuInfo) {
		super.onCreateContextMenu(menu, v, menuInfo)

		if (contextMenuResource != 0) {
			menuInflater.inflate(contextMenuResource, menu)
			changeContextMenu(v, menu)
		}
	}

	override fun onDestroy() {
		super.onDestroy()
		api.cancel()
	}

	fun back(@Suppress(ON_CLICK) view: View) {
		back()
	}

	fun logout(@Suppress(ON_CLICK) view: View) {
		logout(api)
	}

	fun close(@Suppress(ON_CLICK) view: View) {
		close()
	}

	fun refresh(@Suppress(ON_CLICK) view: View) {
		refresh()
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

	protected fun getExtraOrUrl(key: String, default: Int?) : String =
			getExtraOrUrl(key, default.toString())

	protected fun getExtraOrUrl(key: String, default: String = "") : String {
		val extras = intent?.extras

		if (extras?.containsKey(key) == true) {
			return extras[key]?.toString() ?: ""
		}

		if (query.containsKey(key)) {
			return query[key] ?: default
		}

		return default
	}
}
