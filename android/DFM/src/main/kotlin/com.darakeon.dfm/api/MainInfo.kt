package com.darakeon.dfm.api

import android.content.Context
import com.darakeon.dfm.BuildConfig
import com.darakeon.dfm.R

object MainInfo
{
	fun getSiteUrl(context: Context) : String {
		if (!BuildConfig.DEBUG)
			return "https://dontflymoney.com/"

		val localAddress =
			context.getString(R.string.local_address)

		return "http://$localAddress/"
	}

	fun getAppVersion(context: Context): String {
		return context.packageManager.getPackageInfo(
			context.packageName,
			0
		).versionName
	}
}
