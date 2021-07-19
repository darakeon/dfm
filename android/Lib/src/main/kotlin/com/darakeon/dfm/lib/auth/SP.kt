package com.darakeon.dfm.lib.auth

import android.content.Context
import com.google.gson.Gson

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

inline fun <reified T> Context.getValueTyped(key: String): T =
	Gson().fromJson(this.getValue(key), T::class.java)

inline fun <reified T> Context.setValueTyped(key: String, value: T) =
	this.setValue(key, Gson().toJson(value))

private val Context.sp
	get() = getSharedPreferences(
		sharedPreferencesDFM,
		Context.MODE_PRIVATE
	)
