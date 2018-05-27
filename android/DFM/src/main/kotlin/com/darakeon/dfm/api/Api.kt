package com.darakeon.dfm.api

import android.app.Activity
import com.darakeon.dfm.R
import com.darakeon.dfm.extensions.isProd
import retrofit2.Call
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class Api(
	private val context: Activity
) {
	private val api: DfmService

	init {
		val retrofit = Retrofit.Builder()
			.baseUrl(getSite())
			.addConverterFactory(GsonConverterFactory.create())
			.build()

		api = retrofit.create(DfmService::class.java)
	}

	private fun getSite() : String {
		val publicDomain = "dontflymoney.com"

		val domain =
			if (context.isProd)
				publicDomain
			else
				context.getString(R.string.local_address)

		val protocol = if (context.isProd) "https" else "http"

		return "$protocol://$domain/"
	}

	private fun <T> Call<Body<T>>.call(
		onSuccess: (T) -> Unit,
		onError: (Throwable) -> Unit
	) {
		val handler = Handler(onSuccess, onError)
		enqueue(handler)
	}

	fun listAccounts(
		username: String,
		onSuccess: (List<Account>) -> Unit,
		onError: (Throwable) -> Unit
	) {
		api.listAccounts(username).call(onSuccess, onError)
	}
}
