package com.darakeon.dfm.api.entities.accounts

import com.google.gson.annotations.SerializedName

data class Account(
	@SerializedName("Name")
	val name: String,

	@SerializedName("Total")
	var total: Double,

	@SerializedName("Url")
	var url: String
)
