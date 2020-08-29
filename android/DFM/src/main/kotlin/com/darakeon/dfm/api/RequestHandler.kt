package com.darakeon.dfm.api

import android.content.Context
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.base.BaseActivity
import okhttp3.Dispatcher
import okhttp3.Interceptor.Chain
import retrofit2.Call

internal class RequestHandler<C>(
	private val caller: C,
	url: String?,
) where C: Context, C: Caller {
	internal val service: RequestService
	private val dispatcher = Dispatcher()
	private var ui: UIHandler? = null

	init {
		val retrofit =
			if (url == null)
				Retrofit.build(caller, dispatcher, this::intercept)
			else
				Retrofit.build(url)

		service = retrofit.create(RequestService::class.java)

		if (caller is BaseActivity)
			ui = UIHandler(caller)
	}

	private fun intercept(chain: Chain) =
		chain.proceed(addAuthTicket(chain))

	private fun addAuthTicket(chain: Chain) =
		chain.request()
			.newBuilder()
			.addHeader("ticket", caller.ticket)
			.build()

	internal fun <T> call(response: Call<Body<T>>, onSuccess: (T) -> Unit) {
		if (Internet.isOffline(caller)) {
			caller.error(R.string.u_r_offline)
			return
		}

		response.enqueue(
			ResponseHandler(caller, ui, onSuccess)
		)

		ui?.startUIWait()
	}

	fun cancel(call: Call<*>?) {
		call?.cancel()
		dispatcher.cancelAll()
		ui?.endUIWait()
	}
}
