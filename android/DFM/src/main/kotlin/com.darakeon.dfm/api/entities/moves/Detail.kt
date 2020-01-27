package com.darakeon.dfm.api.entities.moves

class Detail {
	var description: String? = null
	var amount: Int = 0
	var value: Double = 0.0

	internal fun equals(description: String?, amount: Int, value: Double): Boolean {
		return this.description == description
				&& this.amount == amount
				&& this.value == value
	}

}
