package com.darakeon.dfm.api

import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.api.entities.accounts.AccountList
import com.darakeon.dfm.api.entities.extract.Extract
import com.darakeon.dfm.api.entities.login.Login
import com.darakeon.dfm.api.entities.moves.Move
import com.darakeon.dfm.api.entities.moves.MoveCreation
import com.darakeon.dfm.api.entities.settings.Settings
import com.darakeon.dfm.api.entities.summary.Summary
import com.darakeon.dfm.api.entities.tfa.TFA
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path
import retrofit2.http.Body as RetrofitBody

internal interface DfmService {
	@GET("api-{ticket}/accounts/list")
	fun listAccounts(
		@Path("ticket") ticket: String
	): Call<Body<AccountList>>

	@GET("api-{ticket}/account-{accountUrl}/moves/extract/{time}")
	fun getExtract(
		@Path("ticket") ticket: String,
		@Path("accountUrl") accountUrl: String,
		@Path("time") time: Int
	): Call<Body<Extract>>

	@POST("api-{ticket}/moves/check/{id}")
	fun check(
		@Path("ticket") ticket: String,
		@Path("id") id: Int
	): Call<Body<Any>>

	@POST("api-{ticket}/moves/uncheck/{id}")
	fun uncheck(
		@Path("ticket") ticket: String,
		@Path("id") id: Int
	): Call<Body<Any>>

	@POST("api-{ticket}/moves/delete/{id}")
	fun delete(
		@Path("ticket") ticket: String,
		@Path("id") id: Int
	): Call<Body<Any>>

	@POST("api/users/login")
	fun login(
		@RetrofitBody login: Login.Request
	): Call<Body<Login.Response>>

	@GET("api-{ticket}/moves/create/{id}")
	fun getMove(
		@Path("ticket") ticket: String,
		@Path("id") id: Int
	): Call<Body<MoveCreation>>

	@POST("api-{ticket}/moves/create/{id}")
	fun saveMove(
		@Path("ticket") ticket: String,
		@Path("id") id: Int,
		@RetrofitBody move: Move
	): Call<Body<Any>>

	@GET("api-{ticket}/users/config")
	fun getConfig(
		@Path("ticket") ticket: String
	): Call<Body<Settings>>

	@POST("api-{ticket}/users/config")
	fun saveConfig(
		@Path("ticket") ticket: String,
		@RetrofitBody settings: Settings
	): Call<Body<Any>>

	@GET("api-{ticket}/account-{accountUrl}/moves/summary/{time}")
	fun getSummary(
		@Path("ticket") ticket: String,
		@Path("accountUrl") accountUrl: String,
		@Path("time") time: Int
	): Call<Body<Summary>>

	@POST("api-{ticket}/account-{accountUrl}/users/tfa")
	fun validateTFA(
		@Path("ticket") ticket: String,
		@RetrofitBody tfa: TFA
	): Call<Body<Any>>

	@GET("api")
	fun wakeupSite(): Call<Body<Any>>
}
