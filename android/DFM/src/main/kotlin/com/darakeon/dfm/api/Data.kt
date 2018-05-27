package com.darakeon.dfm.api

import com.google.gson.annotations.SerializedName

data class Data<T>(
	@SerializedName("AccountList")
	val accountList: T
)
