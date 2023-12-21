package com.darakeon.dfm.lib.api

import android.content.Context
import com.darakeon.dfm.lib.BuildConfig
import com.darakeon.dfm.lib.R

object MainInfo {
	fun getSiteUrl(context: Context) : String {
		val siteAddress =
			if (Internet.isEmulator())
				"10.0.2.2"
			else
				context.getString(R.string.site_address)

		return when {
			siteAddress == "" -> ""
			BuildConfig.DEBUG -> "http://$siteAddress:2312/"
			else -> "https://$siteAddress/"
		}
	}

	private fun isIP(address: String) =
		Regex("\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}")
			.matches(address)

	fun getAppVersion(context: Context): String {
		return context.packageManager.getPackageInfo(
			context.packageName,
			0
		).versionName
	}
}
