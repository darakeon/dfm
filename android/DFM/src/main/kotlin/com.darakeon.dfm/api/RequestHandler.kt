package com.darakeon.dfm.api

import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.dialogs.alertError
import com.darakeon.dfm.extensions.isProd
import okhttp3.Dispatcher
import okhttp3.Interceptor
import okhttp3.OkHttpClient
import okhttp3.Response
import retrofit2.Call
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

internal open class RequestHandler(
	private val activity: BaseActivity
) {
	internal val service: RequestService
	private val dispatcher = Dispatcher()

	init {
		val retrofit = getConfig()
		service = retrofit.create(RequestService::class.java)
	}

	private fun getConfig(): Retrofit {
		val clientBuilder = OkHttpClient.Builder()
		clientBuilder.dispatcher(dispatcher)
		clientBuilder.addInterceptor(this::addAuthTicket)

		return Retrofit.Builder()
			.baseUrl(getSite())
			.client(clientBuilder.build())
			.addConverterFactory(GsonConverterFactory.create())
			.build()
	}

	private fun addAuthTicket(chain: Interceptor.Chain): Response {
		val request = chain.request()
			.newBuilder()
			.addHeader("ticket", activity.ticket)
			.build()

		return chain.proceed(request)
	}

	private fun getSite() : String {
		val publicDomain = "dontflymoney.com"

		val domain =
			if (activity.isProd)
				publicDomain
			else
				activity.getString(R.string.local_address)

		val protocol = if (activity.isProd) "https" else "http"

		return "$protocol://$domain/"
	}

	internal fun call(response: Call<Body<Any>>, onSuccess: () -> Unit) {
		val onSuccessAny: (Any) -> Unit = { onSuccess() }
		call(response, onSuccessAny)
	}

	internal fun <T> call(response: Call<Body<T>>, onSuccess: (T) -> Unit) {
		if (Internet.isOffline(activity)) {
			val error = activity.getString(R.string.u_r_offline)
			activity.alertError(error)
			return
		}

		val uiHandler = UIHandler(activity)

		val responseHandler = ResponseHandler(
			activity,
			uiHandler,
			onSuccess
		)

		response.enqueue(responseHandler)

		uiHandler.startUIWait()
	}

	fun cancel() {
		dispatcher.cancelAll()
	}
}
