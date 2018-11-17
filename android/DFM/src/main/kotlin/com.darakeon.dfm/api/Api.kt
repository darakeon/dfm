package com.darakeon.dfm.api

import android.app.Activity
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.api.entities.accounts.AccountList
import com.darakeon.dfm.api.entities.extract.Extract
import com.darakeon.dfm.api.entities.login.Login
import com.darakeon.dfm.api.entities.moves.Move
import com.darakeon.dfm.api.entities.moves.MoveCreation
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
		id: Int,
		onSuccess: () -> Unit
	) {
		api.check(authKey, id).call(onSuccess)
	}

	fun uncheck(
		authKey: String,
		id: Int,
		onSuccess: () -> Unit
	) {
		api.uncheck(authKey, id).call(onSuccess)
	}

	fun delete(
		ticket: String,
		id: Int,
		onSuccess: () -> Unit
	) {
		api.delete(ticket, id).call(onSuccess)
	}

	fun login(
		login: Login.Request,
		onSuccess: (Login.Response) -> Unit
	) {
		api.login(login).call(onSuccess)
	}

	fun getMove(
		ticket: String,
		accountUrl: String?,
		id: Int,
		onSuccess: (MoveCreation) -> Unit
	) {
		if (accountUrl == null)
			api.getMove(ticket, id).call(onSuccess)
		else
			api.getMove(ticket, accountUrl, id).call(onSuccess)
	}

	fun saveMove(
		ticket: String,
		move: Move,
		onSuccess: () -> Unit
	) {
		api.saveMove(ticket, move.id, move).call(onSuccess)
	}
}
