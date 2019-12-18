package com.darakeon.dfm.api.entities.moves

enum class Nature(val value: Int) {
	Out(0),
	In(1),
	Transfer(2);

	companion object {
		fun get(value: Int?) =
			values().singleOrNull {
				it.value == value
			}
	}
}
