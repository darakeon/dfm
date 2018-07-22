package com.darakeon.dfm.api.old.entities

enum class Nature {
	Out,
	In,
	Transfer;

	fun getNumber(): Int =
		when (this) {
			Out -> 0
			In -> 1
			Transfer -> 2
		}

	companion object {

		fun getNature(number: Int?): Nature =
			when (number) {
				0 -> Out
				1 -> In
				2 -> Transfer
				else -> throw UnsupportedOperationException()
			}
	}
}
