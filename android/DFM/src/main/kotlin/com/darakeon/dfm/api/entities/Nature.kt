package com.darakeon.dfm.api.entities

enum class Nature {
	Out,
	In,
	Transfer;

	fun GetNumber(): Int {
		return when (this) {
			Out -> 0
			In -> 1
			Transfer -> 2
		}
	}

	companion object {

		fun GetNature(number: Int?): Nature {
			return when (number) {
				0 -> Out
				1 -> In
				2 -> Transfer
				else -> throw UnsupportedOperationException()
			}
		}
	}
}
