package com.darakeon.dfm.api.entities

import com.google.gson.annotations.SerializedName
import java.util.*

data class Move(
	@SerializedName("Description")
	val description: String,

	@SerializedName("Date")
	val date: Calendar,

	@SerializedName("Total")
	val total: Double,

	@SerializedName("Checked")
	val checked: Boolean,

	@SerializedName("ID")
	val id: Int
)