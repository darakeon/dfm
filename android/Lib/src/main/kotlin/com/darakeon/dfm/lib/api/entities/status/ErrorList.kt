package com.darakeon.dfm.lib.api.entities.status

data class ErrorList(
	val logs: List<ErrorLog> = emptyList()
)
