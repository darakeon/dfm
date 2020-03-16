package com.darakeon.dfm.api

import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.dialogs.alertError
import okhttp3.Dispatcher
import okhttp3.Interceptor.Chain
import retrofit2.Call

internal class RequestHandler(
	private val activity: BaseActivity
) {
	internal val service: RequestService
	private val dispatcher = Dispatcher()
	private val uiHandler = UIHandler(activity)

	init {
		val url = activity.serverUrl

		val retrofit =
			if (url == null)
				Retrofit.build(activity, dispatcher, this::intercept)
			else
				Retrofit.build(url)

		service = retrofit.create(RequestService::class.java)
	}

	private fun intercept(chain: Chain) =
		chain.proceed(addAuthTicket(chain))

	private fun addAuthTicket(chain: Chain) =
		chain.request()
			.newBuilder()
			.addHeader("ticket", activity.ticket)
			.build()

	internal fun <T> call(response: Call<Body<T>>, onSuccess: (T) -> Unit) {
		if (Internet.isOffline(activity)) {
			val error = activity.getString(R.string.u_r_offline)
			activity.alertError(error)
			return
		}

		response.enqueue(ResponseHandler(
			activity,
			uiHandler,
			onSuccess
		))

		uiHandler.startUIWait()
	}

	fun cancel(call: Call<*>?) {
		call?.cancel()
		dispatcher.cancelAll()
		uiHandler.endUIWait()
	}
}
