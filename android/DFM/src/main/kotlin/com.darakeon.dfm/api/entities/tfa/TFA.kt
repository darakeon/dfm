package com.darakeon.dfm.api.entities.tfa

import com.google.gson.annotations.SerializedName

data class TFA(
	@SerializedName("Code")
	val code: String
)
