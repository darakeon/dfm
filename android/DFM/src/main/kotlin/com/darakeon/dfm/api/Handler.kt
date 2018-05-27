package com.darakeon.dfm.api

import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

internal class Handler<T>(
	private val onSuccess: (T) -> Unit,
	private val onError: (Throwable) -> Unit
)  : Callback<Body<T>> {
	override fun onResponse(call: Call<Body<T>>?, response: Response<Body<T>>?) {
		if (response == null) {
			onError(Exception("Null response"))
			return
		}

		val body = response.body()

		if (body == null) {
			onError(Exception("Null body"))
			return
		}

		if (body.data == null) {
			onError(Exception("[${body.code}] ${body.error}"))
			return
		}

		onSuccess(body.data.accountList)
	}

	override fun onFailure(call: Call<Body<T>>?, throwable: Throwable?) =
		if (throwable == null) {
			onError(Exception("Null error"))
		} else {
			onError(throwable)
		}
}
