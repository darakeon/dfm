package com.darakeon.dfm.lib.api.entities.signup

data class SignUp (
	val email: String,
	val password: String,
	val acceptedContract: Boolean,
	val language: String,
	val timeZone: String,
)
