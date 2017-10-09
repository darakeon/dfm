package com.darakeon.dfm.activities.base

import android.content.Context
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
import com.darakeon.dfm.api.Step
import com.darakeon.dfm.user.Authentication
import com.darakeon.dfm.user.Language
import com.darakeon.dfm.user.Theme
import org.json.JSONObject

abstract class SmartActivity<T : SmartStatic>(var static : T) : FixOrientationActivity() {

	var clickedView: View? = null

	var succeeded: Boolean
		get() = static.succeeded
		set(value) { static.succeeded = value }

	protected abstract fun contentView(): Int
	protected open fun optionsMenuResource(): Int {
		return 0
	}

	protected open fun contextMenuResource(): Int {
		return 0
	}

	protected open fun viewWithContext(): Int {
		return 0
	}

	protected open val isLoggedIn: Boolean
		get() = true

	protected val hasParent: Boolean
		get() = false

	protected open val hasTitle: Boolean
		get() = true

	protected open fun changeContextMenu(view: View, menuInfo: ContextMenu) {}

	protected val Authentication: Authentication get() = Authentication(this)

	val message: Message<T> get() = Message(this)
	protected val navigation: Navigation<T> get() = Navigation(this, Authentication)
	internal val resultHandler: ResultHandler<T> get() = ResultHandler(this, navigation)

	var request: InternalRequest<T>? = null

	override fun onCreate(savedInstanceState: Bundle?) {
		Language.ChangeFromSaved(this)
		Theme.ChangeFromSaved(this)

		super.onCreate(savedInstanceState)

		if (!hasTitle)
			requestWindowFeature(Window.FEATURE_NO_TITLE)

		static.inflater = getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

		setContentView(contentView())
		setupActionBar()

		if (viewWithContext() != 0) {
			val contextView = findViewById(viewWithContext())
			registerForContextMenu(contextView)
		}

		findViewById(R.id.highlight)?.
			setBackgroundColor(Theme.getHighLightColor())
	}

	override fun onCreateOptionsMenu(menu: Menu): Boolean {
		super.onCreateOptionsMenu(menu)

		if (optionsMenuResource() != 0)
			menuInflater.inflate(optionsMenuResource(), menu)

		val bottomMenu = findViewById(R.id.menu) as LinearLayout?

		if (bottomMenu != null)
		{
			(0..bottomMenu.childCount - 1).forEach {
				val button = bottomMenu.getChildAt(it) as Button
				button.applyGlyphicon(this)

				if (this is AccountsActivity)
					findViewById(R.id.action_home).isEnabled = false

				if (this is SettingsActivity)
					findViewById(R.id.action_settings).isEnabled = false

				if (this is MovesCreateActivity)
					findViewById(R.id.action_move).isEnabled = false
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

	override fun onDestroy() {
		super.onDestroy()
		request?.Cancel()
	}

	private fun setupActionBar() {
		if (hasParent) {
			val actionBar = actionBar

			actionBar?.setDisplayHomeAsUpEnabled(true)
		}
	}


	fun back(@Suppress(onClick) view: View) {
		navigation.back()
	}

	fun logout(@Suppress(onClick) view: View) {
		navigation.logout()
	}

	fun close(@Suppress(onClick) view: View) {
		navigation.close()
	}

	fun refresh(@Suppress(onClick) view: View) {
		refresh()
	}

	fun goToAccounts(@Suppress(onClick) view: View) {
		navigation.redirect(AccountsActivity::class.java)
	}

	fun goToSettings(@Suppress(onClick) view: View) {
		navigation.goToSettings()
	}

	fun createMove(@Suppress(onClick) view: View) {
		navigation.createMove()
	}

	protected fun createMove(extras: Bundle) {
		navigation.createMove(extras)
	}

	fun refresh() {
		finish()
		startActivity(intent)
	}


	abstract fun HandleSuccess(data: JSONObject, step: Step)

	fun HandlePostResult(result: JSONObject, step: Step) {
		resultHandler.HandlePostResult(result, step)
		succeeded = true
	}

	fun HandlePostError(error: String) {
		succeeded = false
		resultHandler.HandlePostError(error)
	}


	open fun EnableScreen() {
		succeeded = true
	}

	fun Reset() {
		succeeded = false
	}
}

const val onClick: String = "UNUSED_PARAMETER"
