package com.darakeon.dfm.extensions

import com.google.gson.Gson

inline fun <reified T> String.fromJson(): T {
	return Gson().fromJson(this, T::class.java)
}

inline fun <reified T> Any.fromJson(): T {
	return toString().fromJson()
}

fun Any.toJson(): String {
	return Gson().toJson(this)
}
