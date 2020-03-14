package com.darakeon.dfm.utils

import com.darakeon.dfm.api.entities.Body
import okhttp3.Request
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class CallMock : Call<Body<String>> {
	private val callbacks = ArrayList<Callback<Body<String>>>()
	private var executed = false
	private var cancelled = false

	private val result: String?
	private val error: Exception?

	constructor() {
		this.result = ""
		this.error = null
	}

	constructor(result: String) {
		this.result = result
		this.error = null
	}

	constructor(error: Exception) {
		this.result = null
		this.error = error
	}

	override fun enqueue(callback: Callback<Body<String>>) {
		callbacks.add(callback)
	}

	override fun isExecuted(): Boolean = executed

	override fun clone(): Call<Body<String>> = this

	override fun isCanceled(): Boolean = cancelled

	override fun cancel() {
		cancelled = true
	}

	override fun execute(): Response<Body<String>>? {
		executed = true

		if (result != null) {
			val body = Body(result, null, null, null)
			val success = Response.success(body)
			callbacks.forEach {
				it.onResponse(this, success)
			}

			return success
		}

		if (error != null) {
			callbacks.forEach {
				it.onFailure(this, error)
			}
		}

		return null
	}

	override fun request(): Request {
		return Request.Builder()
			.url("http://dontflymoney.com/tests")
			.build()
	}
}
