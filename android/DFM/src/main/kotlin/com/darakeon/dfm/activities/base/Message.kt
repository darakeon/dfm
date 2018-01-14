package com.darakeon.dfm.activities.base

import android.app.Activity
import android.app.AlertDialog
import android.content.DialogInterface
import android.content.DialogInterface.OnClickListener
import com.darakeon.dfm.R

val cancelClickListener: OnClickListener = OnClickListener {
	dialog, _ -> dialog.cancel()
}

fun Activity.alertYesNo(message: String, answer: IYesNoDialogAnswer) {

	val listener = OnClickListener { dialog, which ->
		when (which) {
			DialogInterface.BUTTON_POSITIVE -> answer.yesAction()
			DialogInterface.BUTTON_NEGATIVE -> answer.noAction()
		}

		dialog.cancel()
	}

	alertError(message, R.string.ok_button, listener, true)
}

fun Activity.alertError(resMessage: Int) {
	alertError(getString(resMessage))
}

fun Activity.alertError(message: String, sendEmailReport: OnClickListener? = null) {
	val clickOk = sendEmailReport ?: cancelClickListener
	val cancelButton = sendEmailReport != null
	val text = if (cancelButton) R.string.send_report_button else R.string.ok_button
	alertError(message, text, clickOk, cancelButton)
}

private fun Activity.alertError(message: String, resOkButton: Int, okClickListener: OnClickListener, hasCancelButton: Boolean) {
	val builder = AlertDialog.Builder(this)
			.setTitle(R.string.error_title)
			.setMessage(message)

	if (resOkButton != 0)
		builder.setPositiveButton(resOkButton, okClickListener)

	if (hasCancelButton)
		builder.setNegativeButton(R.string.cancel_button, cancelClickListener)

	builder.show()
}


fun Activity.createWaitDialog(): AlertDialog? {
	return AlertDialog.Builder(this)
		.setTitle(getString(R.string.wait_title))
		.setMessage(R.string.wait_text)?.show()
}
