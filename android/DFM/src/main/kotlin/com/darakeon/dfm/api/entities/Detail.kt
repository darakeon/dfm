package com.darakeon.dfm.api.entities

class Detail {
	var description: String? = null
	var amount: Int = 0
	var value: Double = 0.toDouble()

	internal fun equals(description: String?, amount: Int, value: Double): Boolean {
		return this.description == description
				&& this.amount == amount
				&& this.value == value
	}

}
