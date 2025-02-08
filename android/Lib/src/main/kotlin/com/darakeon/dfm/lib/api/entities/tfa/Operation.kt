package com.darakeon.dfm.lib.api.entities.tfa

enum class Operation(val value: Int) {
	Validate(0),
	AskRemove(1),
	;

	companion object {
		fun get(value: Int?) =
			entries.singleOrNull {
				it.value == value
			}
	}
}
