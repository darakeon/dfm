package com.darakeon.dfm.testutils.api

import com.darakeon.dfm.lib.api.entities.Body
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.Request
import okhttp3.ResponseBody
import okio.Timeout
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

open class CallMock<Data>(
	private val newBody: (Data) -> Body<Data>,
	private val result: Data?,
	private val error: Exception?,
) : Call<Body<Data>> {
	private val callbacks = ArrayList<Callback<Body<Data>>>()
	private var executed = false
	private var cancelled = false

	protected constructor(newBody: (Data) -> Body<Data>)
		: this(newBody, null, null)

	protected constructor(newBody: (Data) -> Body<Data>, result: Data)
		: this(newBody, result, null)

	protected constructor(newBody: (Data) -> Body<Data>, error: Exception)
		: this(newBody, null, error)

	override fun enqueue(callback: Callback<Body<Data>>) {
		callbacks.add(callback)
	}

	override fun isExecuted() = executed

	override fun clone() = this

	override fun isCanceled() = cancelled

	override fun cancel() {
		cancelled = true
	}

	override fun execute(): Response<Body<Data>> {
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
			.url("http://api.dontflymoney.com/tests")
			.build()
	}

	override fun timeout() = Timeout()
}
