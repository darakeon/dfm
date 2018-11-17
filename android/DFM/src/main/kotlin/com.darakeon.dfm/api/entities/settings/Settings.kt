package com.darakeon.dfm.api.entities.settings

import com.google.gson.annotations.SerializedName

data class Settings(
	@SerializedName("UseCategories")
	var useCategories: Boolean,

	@SerializedName("MoveCheck")
	var moveCheck: Boolean
)
