package com.darakeon.dfm.lib.api.entities.status

data class ErrorList(
	val count: Int = 0,
	val logs: List<ErrorLog> = emptyList()
)
