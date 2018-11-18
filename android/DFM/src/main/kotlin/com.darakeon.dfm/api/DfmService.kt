package com.darakeon.dfm.api

import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.api.entities.TFA
import com.darakeon.dfm.api.entities.accounts.AccountList
import com.darakeon.dfm.api.entities.extract.Extract
import com.darakeon.dfm.api.entities.login.Login
import com.darakeon.dfm.api.entities.moves.Move
import com.darakeon.dfm.api.entities.moves.MoveCreation
import com.darakeon.dfm.api.entities.settings.Settings
import com.darakeon.dfm.api.entities.summary.Summary
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path
import retrofit2.http.Body as RetrofitBody

internal interface DfmService {
	@GET("Api-{ticket}/accounts/list")
	fun listAccounts(
		@Path("ticket") ticket: String
	): Call<Body<AccountList>>

	@GET("Api-{ticket}/Account-{accountUrl}/moves/extract/{time}")
	fun getExtract(
		@Path("ticket") ticket: String,
		@Path("accountUrl") accountUrl: String,
		@Path("time") time: Int
	): Call<Body<Extract>>

	@POST("Api-{ticket}/moves/check/{id}")
	fun check(
		@Path("ticket") ticket: String,
		@Path("id") id: Int
	): Call<Body<Any>>

	@POST("Api-{ticket}/moves/uncheck/{id}")
	fun uncheck(
		@Path("ticket") ticket: String,
		@Path("id") id: Int
	): Call<Body<Any>>

	@POST("Api-{ticket}/moves/delete/{id}")
	fun delete(
		@Path("ticket") ticket: String,
		@Path("id") id: Int
	): Call<Body<Any>>

	@POST("Api/Users/Login")
	fun login(
		@RetrofitBody login: Login.Request
	): Call<Body<Login.Response>>

	@GET("Api-{ticket}/moves/create/{id}")
	fun getMove(
		@Path("ticket") ticket: String,
		@Path("id") id: Int
	): Call<Body<MoveCreation>>

	@GET("Api-{ticket}/Account-{accountUrl}/moves/create/{id}")
	fun getMove(
		@Path("ticket") ticket: String,
		@Path("accountUrl") accountUrl: String,
		@Path("id") id: Int
	): Call<Body<MoveCreation>>

	@POST("Api-{ticket}/moves/create/{id}")
	fun saveMove(
		@Path("ticket") ticket: String,
		@Path("id") id: Int,
		@RetrofitBody move: Move
	): Call<Body<Any>>

	@GET("Api-{ticket}/users/config")
	fun getConfig(
		@Path("ticket") ticket: String
	): Call<Body<Settings>>

	@POST("Api-{ticket}/users/config")
	fun saveConfig(
		@Path("ticket") ticket: String,
		@RetrofitBody settings: Settings
	): Call<Body<Any>>

	@GET("Api-{ticket}/Account-{accountUrl}/moves/summary/{time}")
	fun getSummary(
		@Path("ticket") ticket: String,
		@Path("accountUrl") accountUrl: String,
		@Path("time") time: Int
	): Call<Body<Summary>>

	@POST("Api-{ticket}/Account-{accountUrl}/users/tfa")
	fun validateTFA(
		@Path("ticket") ticket: String,
		@RetrofitBody tfa: TFA
	): Call<Body<Any>>

	@GET("Api")
	fun wakeupSite(): Call<Body<Any>>
}
