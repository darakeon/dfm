package com.darakeon.dfm.activities.base

import android.app.Activity
import android.app.AlertDialog
import android.app.Dialog
import android.app.ProgressDialog
import android.content.DialogInterface
import android.content.DialogInterface.OnClickListener
import android.widget.TextView
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

fun Activity.alertError(message: String) {
	alertError(message, R.string.ok_button, cancelClickListener, false)
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


internal val Activity.waitDialog: Dialog
	get() {
		val progress = Dialog(this)
		progress.setTitle(getString(R.string.wait_title))

		val message = TextView(this)
		message.text = getString(com.darakeon.dfm.R.string.wait_text)
		progress.setContentView(message)

		return progress
	}

fun Activity.showWaitDialog(): Dialog {
	val progress = waitDialog
	progress.show()

	return progress
}
