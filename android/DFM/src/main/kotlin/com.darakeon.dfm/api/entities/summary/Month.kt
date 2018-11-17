package com.darakeon.dfm.api.entities.summary

import com.google.gson.annotations.SerializedName

data class Month(
	@SerializedName("Name")
	val name: String,

	@SerializedName("Number")
	val number: Int,

	@SerializedName("Total")
	val total: Double
)
