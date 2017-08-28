package com.darakeon.dfm.activities.base

import com.darakeon.dfm.user.DfmLicenseCheckerCallback
import com.darakeon.dfm.user.Unique
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.WelcomeActivity
import com.google.android.vending.licensing.AESObfuscator
import com.google.android.vending.licensing.LicenseChecker
import com.google.android.vending.licensing.ServerManagedPolicy

class License(private val activity: WelcomeActivity) {
    private val checker: LicenseChecker
    private val callback: DfmLicenseCheckerCallback

    init {
        callback = DfmLicenseCheckerCallback(activity)

        val SALT = byteArrayOf(-21, +84, -38, +79, -81, -74, -98, +73, -14, +93, -27, -94, -87, -32, +57, +42, +62, -78, -54, -29)
        val appKey = activity.getString(R.string.license_key)

        val obfuscator = AESObfuscator(SALT, activity.packageName, Unique.GetKey(activity))
        val policy = ServerManagedPolicy(activity, obfuscator)
        checker = LicenseChecker(activity, policy, appKey)
    }

    fun Check() {
        activity.Reset()
        checker.checkAccess(callback)
    }

    internal fun Destroy() {
        checker.onDestroy()
    }


}
