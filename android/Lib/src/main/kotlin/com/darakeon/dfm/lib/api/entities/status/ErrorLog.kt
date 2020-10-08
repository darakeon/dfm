package com.darakeon.dfm.lib.api.entities.status

data class ErrorLog(
	val id: String,
	val exception: Except,
) {
	fun id(): Int {
		if (!isDate()) return 0
		return id.substring(11).toInt()
	}

	fun date(): String {
		if (!regex.matches(id)) return id
		val replacer = "$1-$2-$3 $4:$5"
		return regex.replace(id, replacer)
	}

	private val regex
		get() = Regex(
			"(\\d{4})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{6})"
		)

	private fun isDate() = regex.matches(id)

	fun message() = exception.mostInner().message
}
