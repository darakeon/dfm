package com.darakeon.dfm.api

import android.app.Activity
import android.app.Dialog
import android.content.pm.ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE
import android.content.pm.ActivityInfo.SCREEN_ORIENTATION_PORTRAIT
import android.content.pm.ActivityInfo.SCREEN_ORIENTATION_REVERSE_LANDSCAPE
import android.content.pm.ActivityInfo.SCREEN_ORIENTATION_REVERSE_PORTRAIT
import android.content.pm.ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED
import android.view.Surface
import android.view.WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON
import com.darakeon.dfm.dialogs.createWaitDialog

open class UIHandler(
	private val activity: Activity
) {
	private var dialog: Dialog? = null
	private var ended = false

	fun startUIWait() {
		openDialog()
		disableSleep()
		disableRotation()
		if (ended) endUIWait()
	}

	private fun openDialog() {
		dialog = activity.createWaitDialog()
	}

	private fun disableSleep() {
		activity.window.addFlags(FLAG_KEEP_SCREEN_ON)
	}

	private fun disableRotation() {
		val currentRotation = activity.windowManager.defaultDisplay.rotation

		activity.requestedOrientation = when (currentRotation) {
			Surface.ROTATION_0 -> SCREEN_ORIENTATION_PORTRAIT
			Surface.ROTATION_90 -> SCREEN_ORIENTATION_LANDSCAPE
			Surface.ROTATION_180 -> SCREEN_ORIENTATION_REVERSE_PORTRAIT
			Surface.ROTATION_270 -> SCREEN_ORIENTATION_REVERSE_LANDSCAPE
			else -> SCREEN_ORIENTATION_UNSPECIFIED
		}
	}

	open fun endUIWait() {
		ended = true
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
