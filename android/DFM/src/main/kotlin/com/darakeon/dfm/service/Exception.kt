package com.darakeon.dfm.service

data class Exception(
	val className: String,
	val message: String,
	val innerException: Exception?,
	val stackTrace: String,
	val source: String,
) {
	fun mostInner(): Exception {
		return innerException?.mostInner() ?: this
	}
}
