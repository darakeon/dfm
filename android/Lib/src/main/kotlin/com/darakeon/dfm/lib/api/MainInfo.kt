package com.darakeon.dfm.lib.api

import android.content.Context
import com.darakeon.dfm.lib.R

object MainInfo
{
	fun getSiteUrl(context: Context) : String {
		val siteAddress =
			context.getString(R.string.site_address)

		return if (isIP(siteAddress))
			"http://$siteAddress/"
		else
			"https://$siteAddress/"
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
