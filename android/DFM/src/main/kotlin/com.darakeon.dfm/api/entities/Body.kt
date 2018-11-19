package com.darakeon.dfm.api.entities

data class Body<T>(
	val data: T?,
	val environment: Environment?,
	val error: String?,
	val code: Int?
)
