package com.darakeon.dfm.extensions

import android.os.Bundle
import com.google.gson.Gson

fun Bundle.putJson(key: String, thing: Any) {
	val json = Gson().toJson(thing)
	putCharSequence(key, json)
}

inline fun <reified T> Bundle.getFromJson(key: String, default: T) : T {
	val json = get(key)?.toString()
	return Gson().fromJson(json, T::class.java) ?: default
}
