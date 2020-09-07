package com.darakeon.dfm.lib.api.entities.status

data class ErrorLog(
	val id: String,
	val exception: Exception
) {
	private val pattern = "(\\d{4})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{6})"
	private val regex = Regex(pattern)

	fun id(): Int {
		if (!regex.matches(id)) return 0
		return id.substring(11).toInt()
	}

	fun title(): String {
		if (!regex.matches(id)) return id
		val replacer = "$1-$2-$3 $4:$5"
		return regex.replace(id, replacer)
	}

	fun message() = exception.mostInner().message
}
