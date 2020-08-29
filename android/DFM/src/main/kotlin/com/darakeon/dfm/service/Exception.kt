package com.darakeon.dfm.service

data class Exception(
	val ClassName: String,
	val Message: String,
	val InnerException: Exception?,
	val StackTraceString: String,
	val Source: String,
) {
	fun mostInner(): Exception {
		return InnerException?.mostInner() ?: this
	}
}
