package com.darakeon.dfm.lib.api

import android.content.Context
import com.google.gson.FieldNamingPolicy
import com.google.gson.Gson
import com.google.gson.GsonBuilder
import okhttp3.Dispatcher
import okhttp3.Interceptor
import okhttp3.OkHttpClient
import okhttp3.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

object Retrofit {
	fun build(
		context: Context,
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
				MainInfo.getSiteUrl(context)
			).client(client)
		}
	}

	fun build(url: String): Retrofit {
		return build { it.baseUrl(url) }
	}

	private fun build(config: (Retrofit.Builder) -> Unit): Retrofit {
		val builder = Retrofit.Builder()
		config(builder)

		val gson: Gson = GsonBuilder()
			.setLenient()
			.setFieldNamingPolicy(FieldNamingPolicy.LOWER_CASE_WITH_DASHES)
			.create()

		builder.addConverterFactory(
			GsonConverterFactory.create(gson)
		)

		val name = "javax.net.ssl.trustStore"
		val value = "NONE"
		System.setProperty(name, value)

		return builder.build()
	}
}
