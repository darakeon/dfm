package com.darakeon.dfm.lib.api.entities.status

data class Except(
	val className: String,
	val message: String,
	val innerException: Except?,
	val stackTrace: String,
	val source: String,
) {
	fun mostInner(): Except {
		return innerException?.mostInner() ?: this
	}
}
