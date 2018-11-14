package com.darakeon.dfm.api.entities

import com.google.gson.annotations.SerializedName

data class Move(
	@SerializedName("Description")
	val description: String,

	@SerializedName("Date")
	val date: Date,

	@SerializedName("Total")
	val total: Double,

	@SerializedName("Checked")
	val checked: Boolean,

	@SerializedName("ID")
	val id: Int
)
