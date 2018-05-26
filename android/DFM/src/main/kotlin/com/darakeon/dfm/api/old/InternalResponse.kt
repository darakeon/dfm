package com.darakeon.dfm.api.old

import org.json.JSONObject

internal class InternalResponse {
	private val result: JSONObject?
	private val error: String?

	constructor(result: JSONObject) {
		this.result = result
		this.error = null
	}

	constructor(error: String) {
		this.result = null
		this.error = error
	}

	fun isSuccess(): Boolean = error == null
	fun getResult(): JSONObject = result!!
	fun getError(): String = error!!
}
