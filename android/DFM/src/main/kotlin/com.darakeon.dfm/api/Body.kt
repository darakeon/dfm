package com.darakeon.dfm.api

data class Body<T>(
	val data: Data<T>?,
	val error: String?,
	val code: Int?
)
