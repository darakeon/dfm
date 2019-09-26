package com.darakeon.dfm.api.entities.extract

import com.darakeon.dfm.api.entities.Date
import com.google.gson.annotations.SerializedName

data class Move(
	@SerializedName("Description")
	val description: String,

	@SerializedName("Year")
	val year: Int,

	@SerializedName("Month")
	val month: Int,

	@SerializedName("Day")
	val day: Int,

	@SerializedName("Total")
	val total: Double,

	@SerializedName("Checked")
	val checked: Boolean,

	@SerializedName("ID")
	val id: Int
) {
	val date: Date
		get() = Date(year, month, day)
}
