package com.darakeon.dfm.extensions

import com.android.volley.VolleyError

val Exception.stackTraceText: String
	get() {
		var stackTrace = ""

		for (trace: StackTraceElement in this.stackTrace) {
			stackTrace += "$trace\n"
		}

		return stackTrace
	}

val VolleyError.info: String
	get() {
		if (this.message != null)
			return this.message ?: ""

		val code = this.networkResponse.statusCode
		val data = String(this.networkResponse.data)
		return "$code: $data"
	}