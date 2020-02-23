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
import android.widget.Toast
import com.darakeon.dfm.R
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.api.Api
import com.darakeon.dfm.auth.Authentication
import com.darakeon.dfm.auth.recoverEnvironment
import com.darakeon.dfm.dialogs.alertError
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.applyGlyphicon
import com.darakeon.dfm.extensions.back
import com.darakeon.dfm.extensions.close
import com.darakeon.dfm.extensions.composeErrorApi
import com.darakeon.dfm.extensions.createMove
import com.darakeon.dfm.extensions.goToSettings
import com.darakeon.dfm.extensions.logout
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.extensions.refresh
import com.darakeon.dfm.moves.MovesCreateActivity
import com.darakeon.dfm.settings.SettingsActivity
import kotlinx.android.synthetic.main.bottom_menu.action_close
import kotlinx.android.synthetic.main.bottom_menu.action_home
import kotlinx.android.synthetic.main.bottom_menu.action_logout
import kotlinx.android.synthetic.main.bottom_menu.action_move
import kotlinx.android.synthetic.main.bottom_menu.action_settings
import kotlinx.android.synthetic.main.bottom_menu.bottom_menu
import java.util.HashMap

abstract class BaseActivity : Activity() {

	var clickedView: View? = null
	private var inflater: LayoutInflater? = null

	private var api: Api? = null
	protected fun callApi(call: (Api) -> Unit) {
		if (api == null) {
			alertError(R.string.error_call_api) {
				it.dismiss()
				composeErrorApi(this)
			}
		} else {
			call(api!!)
		}
	}

	private var auth: Authentication? = null

	var ticket: String
		get() = auth?.ticket ?: ""
		set(value) { auth?.ticket = value }

	fun clearAuth() = auth?.clear()

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
		recoverEnvironment()

		super.onCreate(savedInstanceState)

		api = Api(this)
		auth = Authentication(this)

		handleScreen()
		setMenuLongClicks()
		processQuery()
	}

	private fun handleScreen() {
		if (!hasTitle)
			requestWindowFeature(Window.FEATURE_NO_TITLE)

		inflater = getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

		if (hasTitle)
			setTitle(title)

		setContentView(contentView)

		if (viewWithContext != null) {
			registerForContextMenu(viewWithContext)
		}
	}

	private fun setMenuLongClicks() {
		action_logout?.setOnLongClickListener {
			logout(api)
			true
		}

		action_close?.setOnLongClickListener {
			close()
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

	override fun onCreateOptionsMenu(menu: Menu): Boolean {
		super.onCreateOptionsMenu(menu)

		if (optionsMenuResource != 0)
			menuInflater.inflate(optionsMenuResource, menu)

		if (bottom_menu != null)
		{
			(0 until bottom_menu.childCount).forEach {
				val button = bottom_menu.getChildAt(it) as Button
				button.applyGlyphicon()

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
		api?.cancel()
	}

	fun back(@Suppress(ON_CLICK) view: View) {
		back()
	}

	fun refresh(@Suppress(ON_CLICK) view: View) {
		refresh()
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

	@Suppress("SameParameterValue")
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
