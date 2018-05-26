package com.darakeon.dfm.base

import android.app.Activity
import android.content.Context
import android.content.pm.ActivityInfo
import android.os.Bundle
import android.view.ContextMenu
import android.view.LayoutInflater
import android.view.Menu
import android.view.View
import android.view.Window
import android.widget.Button
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.auth.getHighLightColor
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
import com.darakeon.dfm.moves.MovesCreateActivity
import com.darakeon.dfm.settings.SettingsActivity
import kotlinx.android.synthetic.main.bottom_menu.action_home
import kotlinx.android.synthetic.main.bottom_menu.action_move
import kotlinx.android.synthetic.main.bottom_menu.action_settings
import kotlinx.android.synthetic.main.bottom_menu.bottom_menu
import java.util.HashMap

abstract class SmartActivity<T : SmartStatic>(var static : T) : Activity() {

	var clickedView: View? = null
	var rotated: Boolean = false
	lateinit var inflater: LayoutInflater

	var succeeded: Boolean
		get() = static.succeeded
		set(value) { static.succeeded = value }

	protected abstract fun contentView(): Int
	protected open fun optionsMenuResource(): Int = 0
	protected open fun contextMenuResource(): Int = 0
	protected open fun viewWithContext(): View? = null
	protected open fun highlight(): View? = null

	protected open val isLoggedIn: Boolean
		get() = true

	protected open val hasTitle: Boolean
		get() = true

	protected open fun changeContextMenu(view: View, menuInfo: ContextMenu) {}

	var request: InternalRequest<T>? = null

	val query = HashMap<String, String>()

	override fun onCreate(savedInstanceState: Bundle?) {
		languageChangeFromSaved()
		themeChangeFromSaved()

		super.onCreate(savedInstanceState)

		rotated =
			(oldConfigInt and ActivityInfo.CONFIG_ORIENTATION) ==
				ActivityInfo.CONFIG_ORIENTATION

		if (!hasTitle)
			requestWindowFeature(Window.FEATURE_NO_TITLE)

		inflater = getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

		setContentView(contentView())

		if (viewWithContext() != null) {
			registerForContextMenu(viewWithContext())
		}

		highlight()?.setBackgroundColor(getHighLightColor())

		val data = intent.data
		if (data?.queryParameterNames != null) {
			for (param: String in data.queryParameterNames) {
				query.put(param, data.getQueryParameter(param))
			}
		}
	}

	override fun onCreateOptionsMenu(menu: Menu): Boolean {
		super.onCreateOptionsMenu(menu)

		if (optionsMenuResource() != 0)
			menuInflater.inflate(optionsMenuResource(), menu)

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

		if (contextMenuResource() != 0) {
			menuInflater.inflate(contextMenuResource(), menu)
			changeContextMenu(v, menu)
		}
	}

	override fun onResume() {
		super.onResume()
		oldConfigInt = 0
	}

	companion object {
		private var oldConfigInt: Int = 0
	}

	override fun onDestroy() {
		super.onDestroy()
		oldConfigInt = changingConfigurations
		request?.cancel()
	}


	fun back(@Suppress(ON_CLICK) view: View) {
		back()
	}

	fun logout(@Suppress(ON_CLICK) view: View) {
		logout()
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

	fun refresh() {
		finish()
		startActivity(intent)
	}

	open fun enableScreen() {
		succeeded = true
	}

	fun reset() {
		succeeded = false
	}

	protected fun getExtraOrUrl(key: String, default: Int?) : String =
			getExtraOrUrl(key, default.toString())

	protected fun getExtraOrUrl(key: String, default: String = "") : String {
		if (intent.extras.containsKey(key)) {
			return intent.extras[key].toString()
		}

		if (query.containsKey(key)) {
			return query[key] ?: default
		}

		return default
	}

}
