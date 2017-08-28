package com.dontflymoney.baseactivity

import android.app.AlertDialog
import android.app.ProgressDialog
import android.content.DialogInterface
import android.content.DialogInterface.OnClickListener
import com.dontflymoney.activityObjects.SmartStatic

import com.dontflymoney.view.R

class Message<T : SmartStatic> internal constructor(private val activity: SmartActivity<T>) {


    fun alertYesNo(message: String, answer: IYesNoDialogAnswer) {
        alertError(message, R.string.ok_button, true, OnClickListener { dialog, which ->
            when (which) {
                DialogInterface.BUTTON_POSITIVE -> answer.YesAction()

                DialogInterface.BUTTON_NEGATIVE -> answer.NoAction()
            }

            dialog.cancel()
        })
    }


    fun alertError(message: Any) {
        alertError(message.toString())
    }

    fun alertError(resMessage: Int, e: Exception) {
        alertError(activity.getString(resMessage) + ": " + e.message)
    }

    private fun alertError(message: String, resOkButton: Int = R.string.ok_button, hasCancelButton: Boolean = false, clickListener: OnClickListener = OnClickListener { dialog, which -> dialog.cancel() }) {
        val builder = AlertDialog.Builder(activity)
                .setTitle(R.string.error_title)
                .setMessage(message)

        if (resOkButton != 0)
            builder.setPositiveButton(resOkButton, clickListener)

        if (hasCancelButton)
            builder.setNegativeButton(R.string.cancel_button, clickListener)

        builder.show()
    }

    fun alertRetryLicense() {
        val message = activity.getString(R.string.license_retry)

        alertError(message, R.string.try_again, true, OnClickListener { dialog, which ->
            dialog.cancel()
            activity.refresh()
        })
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
