package com.darakeon.dfm.activities.base

import android.app.Activity

val Activity.installer: String?
	get() = packageManager.getInstallerPackageName(packageName)

private val googlePlay = "com.android.vending"

val Activity.isProd: Boolean
	get() = installer == googlePlay
