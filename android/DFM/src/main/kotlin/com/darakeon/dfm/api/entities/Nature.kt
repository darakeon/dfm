package com.darakeon.dfm.api.entities

enum class Nature {
	Out,
	In,
	Transfer;

	fun GetNumber(): Int {
		when (this) {
			Out -> return 0
			In -> return 1
			Transfer -> return 2
			else -> throw UnsupportedOperationException()
		}
	}

	companion object {

		fun GetNature(number: Int?): Nature {
			when (number) {
				0 -> return Out
				1 -> return In
				2 -> return Transfer
				else -> throw UnsupportedOperationException()
			}
		}
	}
}
