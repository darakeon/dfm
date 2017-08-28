package com.darakeon.dfm.user

import android.content.Intent
import android.net.Uri
import com.darakeon.dfm.api.Site
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.WelcomeActivity
import com.google.android.vending.licensing.LicenseCheckerCallback
import com.google.android.vending.licensing.Policy

class DfmLicenseCheckerCallback(private val activity: WelcomeActivity) : LicenseCheckerCallback {

	override fun allow(reason: Int) {
		if (activity.isFinishing) {
			// Don't update UI if Activity is finishing.
			return
		}

		if (reason == Policy.RETRY) {
			activity.message.alertRetryLicense()
		} else {
			activity.runOnUiThread { activity.GoToApp() }
		}
	}

	override fun dontAllow(reason: Int) {
		if (activity.isFinishing) {
			// Don't update UI if Activity is finishing.
			return
		}

		if (reason == Policy.RETRY) {
			activity.message.alertRetryLicense()
		} else if (Site.IsLocal()) {
			activity.runOnUiThread { activity.KillThem() }
		} else {
			val intent = Intent(Intent.ACTION_VIEW)
			intent.data = Uri.parse("market://details?id=" + activity.packageName)
			activity.startActivity(intent)
		}
	}

	override fun applicationError(errorCode: Int) {
		if (activity.isFinishing) {
			// Don't update UI if Activity is finishing.
			return
		}

		val genericMessage = activity.getString(R.string.license_error)
		val specificMessage = String.format(genericMessage, errorCode)

		activity.message.alertError(specificMessage)
	}

}
