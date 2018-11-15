package com.darakeon.dfm.api.entities.moves

import com.google.gson.annotations.SerializedName

class Detail {
	@SerializedName("Description")
	var description: String? = null

	@SerializedName("Amount")
	var amount: Int = 0

	@SerializedName("Value")
	var value: Double = 0.0

	internal fun equals(description: String?, amount: Int, value: Double): Boolean {
		return this.description == description
				&& this.amount == amount
				&& this.value == value
	}

}
