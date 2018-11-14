package com.darakeon.dfm.api

import android.app.Activity
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.AccountList
import com.darakeon.dfm.api.entities.Extract
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

	private fun Call<Body<Any>>.call(onSuccess: () -> Unit) {
		val onSuccessAny: (Any) -> Unit = { onSuccess() }
		call(onSuccessAny)
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
		onSuccess: (AccountList) -> Unit
	) {
		api.listAccounts(authKey).call(onSuccess)
	}

	fun getExtract(
		authKey: String,
		accountUrl: String,
		time: Int,
		onSuccess: (Extract) -> Unit
	) {
		api.getExtract(authKey, accountUrl, time).call(onSuccess)
	}

	fun check(
		authKey: String,
		accountUrl: String,
		id: Int,
		onSuccess: () -> Unit
	) {
		api.check(authKey, accountUrl, id).call(onSuccess)
	}

	fun uncheck(
		authKey: String,
		accountUrl: String,
		id: Int,
		onSuccess: () -> Unit
	) {
		api.uncheck(authKey, accountUrl, id).call(onSuccess)
	}

	fun delete(
		authKey: String,
		accountUrl: String,
		id: Int,
		onSuccess: () -> Unit
	) {
		api.delete(authKey, accountUrl, id).call(onSuccess)
	}
}
