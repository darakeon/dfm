package com.darakeon.dfm.api

import android.app.Activity
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Account
import com.darakeon.dfm.dialogs.alertError
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

	private fun <T> Call<Body<T>>.call(onSuccess: (T) -> Unit) {
		if (Internet.isOffline(context)) {
			val error = context.getString(R.string.u_r_offline)
			context.alertError(error)
			return
		}

		val handler = Handler(context, onSuccess)

		enqueue(handler)

		context.startUIWait()
	}

	fun listAccounts(
		authKey: String,
		onSuccess: (Array<Account>) -> Unit
	) {
		api.listAccounts(authKey).call(onSuccess)
	}
}
