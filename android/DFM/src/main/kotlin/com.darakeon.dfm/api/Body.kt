package com.darakeon.dfm.api

data class Body<T>(
	val data: T?,
	val error: String?,
	val code: Int?
)
