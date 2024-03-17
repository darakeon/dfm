package com.darakeon.dfm.lib.api

import android.content.Context
import com.darakeon.dfm.lib.api.entities.Body
import okhttp3.Dispatcher
import okhttp3.Interceptor.Chain
import retrofit2.Call

class RequestHandler<C>(
	private val caller: C,
	url: String?,
) where C: Context, C: ApiCaller {
	internal val service: RequestService
	private val dispatcher = Dispatcher()

	init {
		val retrofit =
			if (url == null || url == "")
				Retrofit.build(caller, dispatcher, this::intercept)
			else
				Retrofit.build(url)

		service = retrofit.create(RequestService::class.java)
	}

	private fun intercept(chain: Chain) =
		chain.proceed(addAuthTicket(chain))

	private fun addAuthTicket(chain: Chain) =
		chain.request()
			.newBuilder()
			.addHeader("ticket", caller.ticket)
			.build()

	fun <T> call(
		call: Call<Body<T>>,
		hasData: Boolean,
		onSuccess: (T?) -> Unit
	) {
		if (Internet.isOffline(caller)) {
			call.cancel()
			caller.offline()
			return
		}

		call.enqueue(
			ResponseHandler(caller, hasData, onSuccess)
		)

		this.isExecuted = { call.isExecuted }
		this.isCancelled = { call.isCanceled }

		caller.startWait()
	}

	fun cancel(call: Call<*>) {
		call.cancel()
		dispatcher.cancelAll()
		caller.endWait()
	}

	val status = {
		when {
			isExecuted() -> Status.Executed
			isCancelled() -> Status.Canceled
			else -> Status.None
		}
	}

	private var isExecuted = { false }
	private var isCancelled = { false }

	enum class Status {
		None, Executed, Canceled
	}
}
