package com.darakeon.dfm.lib.auth

import android.content.Context

private const val sharedPreferencesDFM = "DfM"

fun Context.getValue(key: String): String {
	val sp = getSharedPreferences(sharedPreferencesDFM, Context.MODE_PRIVATE)
	return sp.getString(key, "") ?: ""
}

fun Context.setValue(key: String, value: String) {
	val sp = getSharedPreferences(sharedPreferencesDFM, Context.MODE_PRIVATE)
	val edit = sp.edit()

	edit.putString(key, value)
	edit.apply()
}
