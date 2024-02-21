package com.darakeon.dfm.dialogs

import android.app.Activity
import android.app.AlertDialog
import com.darakeon.dfm.R

fun Activity.confirm(message: String, okClick: () -> Unit) {
	alert(message, R.string.ok_button, true) { okClick() }
}

fun Activity.confirm(resMessage: Int, okClick: () -> Unit) {
	confirm(getString(resMessage), okClick)
}

fun Activity.alertError(resMessage: Int) {
	alertError(getString(resMessage))
}

fun Activity.alertError(message: String) {
	alert(message, R.string.ok_button, false) { }
}

fun Activity.alertError(resMessage: Int, resAction: Int, action: () -> Unit) {
	alert(
		getString(resMessage),
		resAction,
		true
	) {
		action()
	}
}

private fun Activity.alert(
	message: String,
	okRes: Int,
	hasCancel: Boolean,
	okClick: () -> Unit
) {
	val builder = AlertDialog.Builder(this)
			.setTitle(R.string.error_title)
			.setMessage(message)

	builder.setPositiveButton(okRes) {
		dialog, _ ->
			dialog.dismiss()
			okClick()
	}

	if (hasCancel) {
		builder.setNegativeButton(R.string.cancel_button) {
			dialog, _ -> dialog.cancel()
		}
	}

	runOnUiThread { builder.show() }
}

fun Activity.createWaitDialog(): AlertDialog? {
	val builder = AlertDialog.Builder(this)
		.setTitle(getString(R.string.wait_title))
		.setMessage(R.string.wait_text)

	var dialog: AlertDialog? = null

	runOnUiThread {
		dialog = builder?.show()
	}

	return dialog
}
