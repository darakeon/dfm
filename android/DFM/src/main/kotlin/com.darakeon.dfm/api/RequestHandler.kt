package com.darakeon.dfm.api

import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.dialogs.alertError
import okhttp3.Dispatcher
import okhttp3.Interceptor.Chain
import okhttp3.OkHttpClient
import retrofit2.Call
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

internal class RequestHandler(
	private val activity: BaseActivity
) {
	internal val service: RequestService
	private val dispatcher = Dispatcher()
	private val uiHandler = UIHandler(activity)

	init {
		service = getRetrofit().create(
			RequestService::class.java
		)
	}

	private fun getRetrofit(): Retrofit {
		val client =
			OkHttpClient.Builder()
				.dispatcher(dispatcher)
				.addInterceptor(this::intercept)
				.build()

		val jsonConverter =
			GsonConverterFactory.create()

		return Retrofit.Builder()
			.baseUrl(MainInfo.getSiteUrl(activity))
			.client(client)
			.addConverterFactory(jsonConverter)
			.build()
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

		val responseHandler = ResponseHandler(
			activity,
			uiHandler,
			onSuccess
		)

		response.enqueue(responseHandler)

		uiHandler.startUIWait()
	}

	fun <T> cancel(call: Call<T>?) {
		call?.cancel()
		dispatcher.cancelAll()
		uiHandler.endUIWait()
	}
}
