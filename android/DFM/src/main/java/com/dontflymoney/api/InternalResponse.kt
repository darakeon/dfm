package com.dontflymoney.api

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


    fun IsSuccess(): Boolean {
        return error == null
    }

    fun GetResult(): JSONObject {
        return result!!
    }

    fun GetError(): String {
        return error!!
    }


}
