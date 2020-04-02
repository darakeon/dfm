package com.darakeon.dfm.extensions

import android.os.Bundle
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import java.lang.reflect.Type

fun Bundle.putJson(key: String, thing: Any) {
	val json = Gson().toJson(thing)
	putCharSequence(key, json)
}

inline fun <reified T> Bundle.getFromJson(key: String, default: T) : T {
	val json = get(key)?.toString()
	val type: Type = object : TypeToken<T>() {}.type
	return Gson().fromJson(json, type) ?: default
}
