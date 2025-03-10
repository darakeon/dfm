package com.darakeon.dfm.lib.utils

import android.app.Activity
import android.app.AlertDialog
import android.os.Bundle
import com.darakeon.dfm.lib.R
import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.api.ApiCaller

class ApiActivity : Activity(), ApiCaller {
	override val ticket: String = "27"
	private var api: Api<ApiActivity>? = null

	private val ui = MockUI()

	class MockUI(
		internal var started: Int = 0,
		internal var ended: Int = 0,
	)

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)
		api = Api(this, null)
		setTheme(R.style.DarkMagic)
	}

	var loggedOut = false
		private set
	override fun logout() {
		loggedOut = true
	}

	var checkedTFA = false
		private set
	override fun checkTFA() {
		checkedTFA = true
	}

	var waitStarted = false
		private set
	override fun startWait() {
		weirdApiTestFix()
		ui.started += 1
		waitStarted = true
	}

	var waitEnded = false
		private set
	override fun endWait() {
		ui.ended += 1
		waitEnded = true
	}

	var showTfaForgottenWarning: Boolean? = null
		private set
	override fun toggleTfaForgottenWarning(show: Boolean) {
		showTfaForgottenWarning = show
	}

	var errorText: String? = null
		private set

	var errorUrl: String? = null
		private set

	var error: Throwable? = null
		private set

	override fun error(text: String) {
		errorText = text
	}

	override fun error(resId: Int) {
		errorText = getString(resId)
	}

	override fun error(resMessage: Int, resButton: Int, action: () -> Unit) {
		errorText = getString(resMessage)
		action()
	}

	override fun error(url: String, error: Throwable) {
		errorUrl = url
		this.error = error
	}

	override fun onDestroy() {
		super.onDestroy()
		api?.cancel()
	}

	private fun weirdApiTestFix() {
		runOnUiThread {
			// I have no idea why, but ApiTest just works if I put this.
			// It almost 6pm, I'm seeing this bug for hours, don't judge me.
			AlertDialog.Builder(this).show()
		}
	}
}
