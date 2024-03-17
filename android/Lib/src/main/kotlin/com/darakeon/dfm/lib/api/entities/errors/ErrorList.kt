package com.darakeon.dfm.lib.api.entities.errors

data class ErrorList(
	val count: Int = 0,
	val logs: List<ErrorLog> = emptyList()
)
