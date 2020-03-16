package com.darakeon.dfm.api

import com.darakeon.dfm.base.BaseActivity
import okhttp3.Dispatcher
import okhttp3.Interceptor
import okhttp3.OkHttpClient
import okhttp3.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

internal object Retrofit {
	fun build(
		activity: BaseActivity,
		dispatcher: Dispatcher,
		interceptor: (Interceptor.Chain) -> Response
	): Retrofit {
		return build {
			val client =
				OkHttpClient.Builder()
					.dispatcher(dispatcher)
					.addInterceptor(interceptor)
					.build()

			it.baseUrl(
				MainInfo.getSiteUrl(activity)
			).client(client)
		}
	}

	fun build(url: String): Retrofit {
		return build { it.baseUrl(url) }
	}

	private fun build(config: (Retrofit.Builder) -> Unit): Retrofit {
		val builder = Retrofit.Builder()

		config(builder)

		val converter = GsonConverterFactory.create()

		return builder
			.addConverterFactory(converter)
			.build()
	}
}
