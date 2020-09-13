package com.darakeon.dfm.base

import android.app.Activity
import android.app.Dialog
import android.content.pm.ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE
import android.content.pm.ActivityInfo.SCREEN_ORIENTATION_PORTRAIT
import android.content.pm.ActivityInfo.SCREEN_ORIENTATION_REVERSE_LANDSCAPE
import android.content.pm.ActivityInfo.SCREEN_ORIENTATION_REVERSE_PORTRAIT
import android.content.pm.ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED
import android.content.res.Configuration.ORIENTATION_LANDSCAPE
import android.content.res.Configuration.ORIENTATION_PORTRAIT
import android.view.Surface.ROTATION_0
import android.view.Surface.ROTATION_180
import android.view.Surface.ROTATION_270
import android.view.Surface.ROTATION_90
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
		val orientation = activity.resources.configuration.orientation
		val rotation = activity.windowManager.defaultDisplay.rotation

		activity.requestedOrientation =
			rotations[rotation]?.get(orientation)
				?: SCREEN_ORIENTATION_UNSPECIFIED
	}

	private val upSide = hashMapOf(
		ORIENTATION_PORTRAIT to SCREEN_ORIENTATION_PORTRAIT,
		ORIENTATION_LANDSCAPE to SCREEN_ORIENTATION_LANDSCAPE
	)

	private val downSide = hashMapOf(
		ORIENTATION_PORTRAIT to SCREEN_ORIENTATION_REVERSE_PORTRAIT,
		ORIENTATION_LANDSCAPE to SCREEN_ORIENTATION_REVERSE_LANDSCAPE
	)

	private val rotations = hashMapOf(
		ROTATION_0 to upSide,
		ROTATION_90 to downSide,
		ROTATION_180 to downSide,
		ROTATION_270 to upSide
	)

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
