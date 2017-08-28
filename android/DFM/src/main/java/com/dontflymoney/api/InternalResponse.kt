package com.dontflymoney.api

import org.json.JSONObject

internal class InternalResponse {
    private val result: JSONObject
    private val error: String?

    constructor(result: JSONObject) {
        this.result = result
    }

    constructor(error: String) {
        this.error = error
    }


    fun IsSuccess(): Boolean {
        return error == null
    }

    fun GetResult(): JSONObject {
        return result
    }

    fun GetError(): String {
        return error
    }


}
