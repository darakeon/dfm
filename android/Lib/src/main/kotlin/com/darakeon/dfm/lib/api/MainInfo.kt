package com.darakeon.dfm.lib.api

import android.content.Context
import com.darakeon.dfm.lib.BuildConfig

object MainInfo
{
	private const val main_address =
		"https://dontflymoney.com/"

	fun getSiteUrl(context: Context) : String {
		if (!BuildConfig.DEBUG)
			return main_address

		val addressResId = context.resources
			.getIdentifier(
				"local_address",
				"string",
				context.packageName
			)

		if (addressResId == 0)
			return main_address

		val localAddress =
			context.getString(addressResId)

		if (localAddress == "")
			return main_address

		return "http://$localAddress/"
	}

	fun getAppVersion(context: Context): String {
		return context.packageManager.getPackageInfo(
			context.packageName,
			0
		).versionName
	}
}
