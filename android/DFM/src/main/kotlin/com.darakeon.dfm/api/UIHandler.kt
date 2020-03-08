package com.darakeon.dfm.api

import android.app.Activity
import android.app.Dialog
import android.content.pm.ActivityInfo
import android.util.Log
import android.view.Surface
import android.view.WindowManager
import com.darakeon.dfm.dialogs.createWaitDialog

class UIHandler(
	private val activity: Activity
) {
	private var dialog: Dialog? = null

	fun startUIWait() {
		openProgressBar()
		disableSleep()
		disableRotation()
	}

	private fun openProgressBar() {
		dialog = activity.createWaitDialog()
	}

	private fun disableSleep() {
		activity.window.addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON)
	}

	private fun disableRotation() {
		val currentRotation = activity.windowManager.defaultDisplay.rotation

		activity.requestedOrientation = when (currentRotation) {
			Surface.ROTATION_0 -> ActivityInfo.SCREEN_ORIENTATION_PORTRAIT
			Surface.ROTATION_90 -> ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE
			Surface.ROTATION_180 -> ActivityInfo.SCREEN_ORIENTATION_REVERSE_PORTRAIT
			Surface.ROTATION_270 -> ActivityInfo.SCREEN_ORIENTATION_REVERSE_LANDSCAPE
			else -> ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED
		}
	}

	fun endUIWait() {
		closeDialog()
		enableSleep()
		enableRotation()
	}

	private fun closeDialog() {
		dialog?.dismiss()
		dialog = null
	}

	private fun enableSleep() {
		activity.window.clearFlags(
			WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON
		)
	}

	private fun enableRotation() {
		activity.requestedOrientation =
			ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED
	}
}
