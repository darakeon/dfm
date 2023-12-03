package com.darakeon.dfm.lib.api

import com.darakeon.dfm.lib.api.entities.Body
import com.darakeon.dfm.lib.api.entities.accounts.AccountList
import com.darakeon.dfm.lib.api.entities.extract.Extract
import com.darakeon.dfm.lib.api.entities.login.Login
import com.darakeon.dfm.lib.api.entities.moves.Lists
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.lib.api.entities.moves.MoveCreation
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.lib.api.entities.settings.Settings
import com.darakeon.dfm.lib.api.entities.status.ErrorList
import com.darakeon.dfm.lib.api.entities.summary.Summary
import com.darakeon.dfm.lib.api.entities.terms.Terms
import com.darakeon.dfm.lib.api.entities.wipe.Wipe
import retrofit2.Call
import retrofit2.http.Field
import retrofit2.http.FormUrlEncoded
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path
import java.util.UUID
import retrofit2.http.Body as RetrofitBody

interface RequestService {
	@GET("accounts/list")
	fun listAccounts(): Call<Body<AccountList>>

	@GET("account-{accountUrl}/moves/extract/{time}")
	fun getExtract(
		@Path("accountUrl") accountUrl: String,
		@Path("time") time: Int
	): Call<Body<Extract>>

	@FormUrlEncoded
	@POST("moves/check/{id}")
	fun check(
		@Path("id") id: UUID,
		@Field("nature") nature: Nature
	): Call<Body<Any>>

	@FormUrlEncoded
	@POST("moves/uncheck/{id}")
	fun uncheck(
		@Path("id") id: UUID,
		@Field("nature") nature: Nature
	): Call<Body<Any>>

	@POST("moves/delete/{id}")
	fun delete(
		@Path("id") id: UUID
	): Call<Body<Any>>

	@FormUrlEncoded
	@POST("users/login")
	fun login(
		@Field("email") email: String,
		@Field("password") password: String
	): Call<Body<Login>>

	@POST("users/logout")
	fun logout(): Call<Body<Any>>

	@GET("moves/create")
	fun getMove(): Call<Body<MoveCreation>>

	@GET("moves/create/{id}")
	fun getMove(
		@Path("id") id: UUID
	): Call<Body<MoveCreation>>

	@POST("moves/create")
	fun saveMove(
		@RetrofitBody move: Move
	): Call<Body<Any>>

	@POST("moves/create/{id}")
	fun saveMove(
		@Path("id") id: UUID,
		@RetrofitBody move: Move
	): Call<Body<Any>>

	@GET("users/getSettings")
	fun getSettings(): Call<Body<Settings>>

	@POST("users/saveSettings")
	fun saveSettings(
		@RetrofitBody settings: Settings
	): Call<Body<Any>>

	@GET("account-{accountUrl}/moves/summary/{time}")
	fun getSummary(
		@Path("accountUrl") accountUrl: String,
		@Path("time") time: Int
	): Call<Body<Summary>>

	@FormUrlEncoded
	@POST("account-{accountUrl}/users/tfa")
	fun validateTFA(
		@Field("code") code: String
	): Call<Body<Any>>

	@GET("moves/lists")
	fun listsForMoves(): Call<Body<Lists>>

	@POST("users/wipe")
	fun wipe(
		@RetrofitBody wipe: Wipe
	): Call<Body<Any>>

	@GET("users/terms")
	fun getTerms(): Call<Body<Terms>>

	@GET("/")
	fun wakeUpSite(): Call<Body<Any>>

	@GET("log/count")
	fun countErrors(): Call<Body<ErrorList>>

	@GET("log/list")
	fun listErrors(): Call<Body<ErrorList>>

	@POST("log/archive/{id}")
	fun archiveErrors(
		@Path("id") id: Int
	): Call<Body<Any>>
}
