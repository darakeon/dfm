package com.darakeon.dfm.extensions

import com.google.gson.Gson

inline fun <reified T> Gson.fromJson(json: Any): T {
	return this.fromJson(json.toString(), T::class.java)
}
