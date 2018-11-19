package com.darakeon.dfm.api.entities

import com.google.gson.annotations.SerializedName

data class Environment(
	@SerializedName("MobileTheme")
	val mobileTheme: String,

	@SerializedName("Language")
	val language: String
)
