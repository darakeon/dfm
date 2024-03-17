package com.darakeon.dfm.lib.api.entities.errors

data class ErrorLog(
	val id: String,
	val handled: Boolean,
	val exception: Except,
	val count: Int,
	val hash: Int,
) {
	fun date(): String {
		if (!regex.matches(id)) return id
		val replacer = "$1-$2-$3 $4:$5"
		return regex.replace(id, replacer)
	}

	private val regex
		get() = Regex(
			"(\\d{4})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{6})"
		)

	fun message() = exception.mostInner().message
}
