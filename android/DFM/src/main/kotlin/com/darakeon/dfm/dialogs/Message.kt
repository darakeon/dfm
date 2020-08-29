package com.darakeon.dfm.dialogs

import android.app.Activity
import android.app.AlertDialog
import com.darakeon.dfm.R

fun Activity.confirm(message: String, okClick: () -> Unit) {
	alert(message, R.string.ok_button, true) { okClick() }
}

fun Activity.alertError(resMessage: Int) {
	alertError(getString(resMessage))
}

fun Activity.alertError(message: String) {
	alert(message, R.string.ok_button, false) { }
}

fun Activity.alertError(resMessage: Int, sendEmailReport: () -> Unit) {
	alert(
		getString(resMessage),
		R.string.send_report_button,
		true
	) {
		sendEmailReport()
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

	builder.show()
}

fun Activity.createWaitDialog(): AlertDialog? {
	return AlertDialog.Builder(this)
		.setTitle(getString(R.string.wait_title))
		.setMessage(R.string.wait_text)
		?.show()
}
