package com.dontflymoney.viewhelper

import android.app.ProgressDialog
import android.content.Intent
import android.net.Uri

import com.dontflymoney.api.Site
import com.dontflymoney.baseactivity.SmartActivity
import com.dontflymoney.view.R
import com.google.android.vending.licensing.LicenseCheckerCallback
import com.google.android.vending.licensing.Policy

class DfmLicenseCheckerCallback(private val activity: SmartActivity, private val progress: ProgressDialog) : LicenseCheckerCallback {

    override fun allow(reason: Int) {
        progress.dismiss()

        if (activity.isFinishing) {
            // Don't update UI if Activity is finishing.
            return
        }

        if (reason == Policy.RETRY) {
            activity.message.alertRetryLicense()
        } else {
            enableScreen()
        }
    }

    private fun enableScreen() {
        activity.runOnUiThread { activity.EnableScreen() }
    }

    override fun dontAllow(reason: Int) {
        progress.dismiss()

        if (activity.isFinishing) {
            // Don't update UI if Activity is finishing.
            return
        }

        if (reason == Policy.RETRY) {
            activity.message.alertRetryLicense()
        } else if (Site.IsLocal()) {
            enableScreen()
        } else {
            val intent = Intent(Intent.ACTION_VIEW)
            intent.data = Uri.parse("market://details?id=" + activity.packageName)
            activity.startActivity(intent)
        }
    }

    override fun applicationError(errorCode: Int) {
        progress.dismiss()

        if (activity.isFinishing) {
            // Don't update UI if Activity is finishing.
            return
        }

        val genericMessage = activity.getString(R.string.license_error)
        val specificMessage = String.format(genericMessage, errorCode)

        activity.message.alertError(specificMessage)
    }

}
