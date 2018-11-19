package com.darakeon.dfm.extensions

val Throwable.stackTraceText: String
	get() {
		var stackTrace = ""

		for (trace: StackTraceElement in this.stackTrace) {
			stackTrace += "$trace\n"
		}

		return stackTrace
	}
