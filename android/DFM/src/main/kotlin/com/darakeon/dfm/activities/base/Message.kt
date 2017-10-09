package com.darakeon.dfm.activities.base

import android.app.AlertDialog
import android.app.ProgressDialog
import android.content.DialogInterface
import android.content.DialogInterface.OnClickListener
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.objects.SmartStatic

class Message<T : SmartStatic> internal constructor(private val activity: SmartActivity<T>) {

	val cancelClickListener: OnClickListener = OnClickListener {
		dialog, _ -> dialog.cancel()
	}


	fun alertYesNo(message: String, answer: IYesNoDialogAnswer) {

		val listener = OnClickListener { dialog, which ->
			when (which) {
				DialogInterface.BUTTON_POSITIVE -> answer.YesAction()
				DialogInterface.BUTTON_NEGATIVE -> answer.NoAction()
			}

			dialog.cancel()
		}

		alertError(message, R.string.ok_button, listener, true)
	}


	fun alertError(resMessage: Int) {
		alertError(activity.getString(resMessage))
	}

	fun alertError(resMessage: Int, e: Exception) {
		alertError(activity.getString(resMessage) + ": " + e.message)
	}

	fun alertError(message: String) {
		alertError(message, R.string.ok_button, cancelClickListener, false)
	}

	fun alertRetryLicense() {
		val message = activity.getString(R.string.license_retry)

		val listener = OnClickListener { dialog, _ ->
			dialog.cancel()
			activity.refresh()
		}

		alertError(message, R.string.try_again, listener, true)
	}

	private fun alertError(message: String, resOkButton: Int, okClickListener: OnClickListener, hasCancelButton: Boolean) {
		val builder = AlertDialog.Builder(activity)
				.setTitle(R.string.error_title)
				.setMessage(message)

		if (resOkButton != 0)
			builder.setPositiveButton(resOkButton, okClickListener)

		if (hasCancelButton)
			builder.setNegativeButton(R.string.cancel_button, cancelClickListener)

		builder.show()
	}


	internal val waitDialog: ProgressDialog
		get() {
			val progress = ProgressDialog(activity)
			progress.setTitle(activity.getString(R.string.wait_title))
			progress.setMessage(activity.getString(R.string.wait_text))

			return progress
		}

	fun showWaitDialog(): ProgressDialog {
		val progress = waitDialog
		progress.show()

		return progress
	}


}
