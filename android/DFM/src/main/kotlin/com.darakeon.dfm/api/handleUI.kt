package com.darakeon.dfm.api

import android.app.Activity
import android.app.Dialog
import android.content.pm.ActivityInfo
import android.content.res.Configuration
import android.view.Surface
import android.view.WindowManager
import com.darakeon.dfm.dialogs.createWaitDialog

private var dialog: Dialog? = null

fun Activity.startUIWait() {
	openProgressBar()
	disableSleep()
	disableRotation()
}

private fun Activity.openProgressBar() {
	dialog = createWaitDialog()
}

private fun Activity.disableSleep() {
	window.addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON)
}

private fun Activity.disableRotation() {
	val rotation = windowManager.defaultDisplay.rotation
	val orientation = resources.configuration.orientation

	requestedOrientation = when (orientation) {
		Configuration.ORIENTATION_PORTRAIT -> handlePortrait(rotation)
		Configuration.ORIENTATION_LANDSCAPE -> handleLandscape(rotation)
		else -> 0
	}
}

private fun handlePortrait(rotation: Int): Int =
	when (rotation) {
		Surface.ROTATION_0, Surface.ROTATION_270 ->
			ActivityInfo.SCREEN_ORIENTATION_PORTRAIT
		else ->
			ActivityInfo.SCREEN_ORIENTATION_REVERSE_PORTRAIT
	}

private fun handleLandscape(rotation: Int): Int =
	when (rotation) {
		Surface.ROTATION_0, Surface.ROTATION_90 ->
			ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE
		else ->
			ActivityInfo.SCREEN_ORIENTATION_REVERSE_LANDSCAPE
	}

fun Activity.endUIWait() {
	closeDialog()
	enableSleep()
	enableRotation()
}

private fun closeDialog() {
	dialog?.dismiss()
	dialog = null
}

private fun Activity.enableSleep() {
	window.clearFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON)
}

private fun Activity.enableRotation() {
	requestedOrientation = ActivityInfo.SCREEN_ORIENTATION_SENSOR
}
