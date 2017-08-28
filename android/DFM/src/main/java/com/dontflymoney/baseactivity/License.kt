package com.dontflymoney.baseactivity

import android.app.ProgressDialog

import com.dontflymoney.userdata.Unique
import com.dontflymoney.view.R
import com.dontflymoney.viewhelper.DfmLicenseCheckerCallback
import com.google.android.vending.licensing.AESObfuscator
import com.google.android.vending.licensing.LicenseChecker
import com.google.android.vending.licensing.ServerManagedPolicy

class License(private val activity: SmartActivity) {
    private val checker: LicenseChecker
    private val callback: DfmLicenseCheckerCallback
    private val progress: ProgressDialog

    init {
        progress = activity.message.getWaitDialog()

        callback = DfmLicenseCheckerCallback(activity, progress)

        val SALT = byteArrayOf(-21, +84, -38, +79, -81, -74, -98, +73, -14, +93, -27, -94, -87, -32, +57, +42, +62, -78, -54, -29)
        val appKey = activity.getString(R.string.license_key)

        val obfuscator = AESObfuscator(SALT, activity.packageName, Unique.GetKey(activity))
        val policy = ServerManagedPolicy(activity, obfuscator)
        checker = LicenseChecker(activity, policy, appKey)
    }

    fun Check() {
        activity.Reset()
        checker.checkAccess(callback)
        progress.show()
    }

    internal fun Destroy() {
        checker.onDestroy()

        if (progress.isShowing)
            progress.dismiss()
    }


}
