package com.darakeon.dfm.testutils.api

import okhttp3.MediaType.Companion.toMediaType
import okhttp3.Request
import okhttp3.ResponseBody
import okio.Timeout
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

open class CallMock<Body>(
	private val newBody: (String) -> Body,
	private val result: String?,
	private val error: Exception?,
) : Call<Body> {
	private val callbacks = ArrayList<Callback<Body>>()
	private var executed = false
	private var cancelled = false

	protected constructor(newBody: (String) -> Body)
		: this(newBody, "", null)

	protected constructor(newBody: (String) -> Body, result: String)
		: this(newBody, result, null)

	protected constructor(newBody: (String) -> Body, error: Exception)
		: this(newBody, null, error)

	override fun enqueue(callback: Callback<Body>) {
		callbacks.add(callback)
	}

	override fun isExecuted() = executed

	override fun clone() = this

	override fun isCanceled() = cancelled

	override fun cancel() {
		cancelled = true
	}

	override fun execute(): Response<Body> {
		executed = true

		if (result != null) {
			val body = newBody(result)
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

		val errorBody = ResponseBody.create(
			"text/plain".toMediaType(), error?.message ?: ""
		)
		return Response.error(500, errorBody)
	}

	override fun request(): Request {
		return Request.Builder()
			.url("http://dontflymoney.com/tests")
			.build()
	}

	override fun timeout() = Timeout()
}
