package com.darakeon.dfm.api.entities

import com.google.gson.annotations.SerializedName

data class ComboItem(
	@SerializedName("Text")
	val text: String,

	@SerializedName("Value")
	val value: String
)
