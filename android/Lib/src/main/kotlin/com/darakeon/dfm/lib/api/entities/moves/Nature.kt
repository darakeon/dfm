package com.darakeon.dfm.lib.api.entities.moves

enum class Nature(val value: Int) {
	Out(0),
	In(1),
	Transfer(2);

	companion object {
		fun get(value: Int?) =
			entries.singleOrNull {
				it.value == value
			}
	}
}
