package com.darakeon.dfm.activities.base

import android.app.Activity
import android.content.Context
import android.content.pm.ActivityInfo
import android.os.Bundle
import android.view.*
import android.widget.Button
import android.widget.LinearLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.AccountsActivity
import com.darakeon.dfm.activities.MovesCreateActivity
import com.darakeon.dfm.activities.SettingsActivity
import com.darakeon.dfm.activities.objects.SmartStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.user.languageChangeFromSaved
import com.darakeon.dfm.user.themeChangeFromSaved
import com.darakeon.dfm.user.getHighLightColor
import java.util.*

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
	protected open fun viewWithContext(): Int = 0

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

		if (viewWithContext() != 0) {
			val contextView = findViewById<View>(viewWithContext())
			registerForContextMenu(contextView)
		}

		findViewById<LinearLayout>(R.id.highlight)
			?.setBackgroundColor(getHighLightColor())

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

		val bottomMenu = findViewById<LinearLayout>(R.id.menu)

		if (bottomMenu != null)
		{
			(0 until bottomMenu.childCount).forEach {
				val button = bottomMenu.getChildAt(it) as Button
				button.applyGlyphicon(this)

				if (this is AccountsActivity)
					findViewById<Button>(R.id.action_home).isEnabled = false

				if (this is SettingsActivity)
					findViewById<Button>(R.id.action_settings).isEnabled = false

				if (this is MovesCreateActivity)
					findViewById<Button>(R.id.action_move).isEnabled = false
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
