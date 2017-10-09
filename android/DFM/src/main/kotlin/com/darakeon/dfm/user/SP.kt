package com.darakeon.dfm.user

import android.content.Context

private val sharedPreferencesDFM = "DfM"

fun Context.getValue(key: String): String {
	val sp = getSharedPreferences(sharedPreferencesDFM, Context.MODE_PRIVATE)
	return sp.getString(key, "")
}

fun Context.setValue(key: String, value: String) {
	val sp = getSharedPreferences(sharedPreferencesDFM, Context.MODE_PRIVATE)
	val edit = sp.edit()

	edit.putString(key, value)
	edit.apply()
}
