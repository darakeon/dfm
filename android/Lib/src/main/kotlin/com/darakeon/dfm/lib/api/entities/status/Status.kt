package com.darakeon.dfm.lib.api.entities.status

@Suppress("unused")
enum class Status(
	val code: Int,
) {
	None(0),
	Online(1),
	DbError(2),
	;

	companion object {
		fun get(value: Int?) =
			entries.singleOrNull {
				it.code == value
			}
	}
}
