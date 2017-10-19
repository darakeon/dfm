package com.darakeon.dfm.api

import org.json.JSONObject

internal class InternalResponse {
	private var result: JSONObject? = null
	private var error: String? = null

	constructor(result: JSONObject) {
		this.result = result
	}

	constructor(error: String) {
		this.error = error
	}


	fun isSuccess(): Boolean = error == null
	fun getResult(): JSONObject = result!!
	fun getError(): String = error!!


}
