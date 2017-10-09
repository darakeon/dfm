package com.darakeon.dfm.user

import android.content.Context

internal object SP {
	private val sharedPreferencesDFM = "DfM"

	fun getValue(context: Context, key: String): String {
		val sp = context.getSharedPreferences(sharedPreferencesDFM, Context.MODE_PRIVATE)
		return sp.getString(key, "")
	}

	fun setValue(context: Context, key: String, value: String) {
		val sp = context.getSharedPreferences(sharedPreferencesDFM, Context.MODE_PRIVATE)
		val edit = sp.edit()

		edit.putString(key, value)
		edit.apply()
	}
}
