package com.darakeon.dfm.service

data class ErrorLog(
	val id: String,
	val exception: Exception
) {
	fun id() = id.substring(11).toInt()

	fun title(): String {
		val pattern = "(\\d{4})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d+)"
		val regex = Regex(pattern)
		val replacer = "$1-$2-$3 $4:$5"
		return regex.replace(id, replacer)
	}

	fun message() = exception.mostInner().message
}
