package com.darakeon.dfm.base

import android.app.Activity
import android.app.Dialog
import android.content.pm.ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED
import android.view.WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON
import com.darakeon.dfm.dialogs.createWaitDialog

open class UIHandler(
	private val activity: Activity
) {
	private var dialog: Dialog? = null
	private var started = 0
	private var ended = 0

	fun startUIWait() {
		started++

		if (ended == started)
			return

		openDialog()
		disableSleep()
		disableRotation()
	}

	private fun openDialog() {
		dialog = activity.createWaitDialog()
	}

	private fun disableSleep() {
		activity.window.addFlags(FLAG_KEEP_SCREEN_ON)
	}

	private fun disableRotation() {
		activity.requestedOrientation =
			activity.resources.configuration.orientation
	}

	open fun endUIWait() {
		ended++

		closeDialog()
		enableSleep()
		enableRotation()
	}

	private fun closeDialog() {
		dialog?.dismiss()
		dialog = null
	}

	private fun enableSleep() {
		activity.window.clearFlags(FLAG_KEEP_SCREEN_ON)
	}

	private fun enableRotation() {
		activity.requestedOrientation = SCREEN_ORIENTATION_UNSPECIFIED
	}
}
