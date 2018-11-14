package com.darakeon.dfm.api.entities

class Login {
	data class Request(
		val email: String,
		val password: String
	)

	data class Response(
		val ticket: String
	)
}
