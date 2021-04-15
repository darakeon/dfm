package com.darakeon.dfm.lib.auth

import android.content.Context

private const val sharedPreferencesDFM = "DfM"

fun Context.getValue(key: String): String {
	return sp.getString(key, "") ?: ""
}

fun Context.setValue(key: String, value: String) {
	sp.edit().apply {
		putString(key, value)
		apply()
	}
}

private val Context.sp
	get() = getSharedPreferences(
		sharedPreferencesDFM,
		Context.MODE_PRIVATE
	)
