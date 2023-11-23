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
	@GET("api/accounts/list")
	fun listAccounts(): Call<Body<AccountList>>

	@GET("api/account-{accountUrl}/moves/extract/{time}")
	fun getExtract(
		@Path("accountUrl") accountUrl: String,
		@Path("time") time: Int
	): Call<Body<Extract>>

	@FormUrlEncoded
	@POST("api/moves/check/{id}")
	fun check(
		@Path("id") id: UUID,
		@Field("nature") nature: Nature
	): Call<Body<Any>>

	@FormUrlEncoded
	@POST("api/moves/uncheck/{id}")
	fun uncheck(
		@Path("id") id: UUID,
		@Field("nature") nature: Nature
	): Call<Body<Any>>

	@POST("api/moves/delete/{id}")
	fun delete(
		@Path("id") id: UUID
	): Call<Body<Any>>

	@FormUrlEncoded
	@POST("api/users/login")
	fun login(
		@Field("email") email: String,
		@Field("password") password: String
	): Call<Body<Login>>

	@POST("api/users/logout")
	fun logout(): Call<Body<Any>>

	@GET("api/moves/create")
	fun getMove(): Call<Body<MoveCreation>>

	@GET("api/moves/create/{id}")
	fun getMove(
		@Path("id") id: UUID
	): Call<Body<MoveCreation>>

	@POST("api/moves/create")
	fun saveMove(
		@RetrofitBody move: Move
	): Call<Body<Any>>

	@POST("api/moves/create/{id}")
	fun saveMove(
		@Path("id") id: UUID,
		@RetrofitBody move: Move
	): Call<Body<Any>>

	@GET("api/users/getSettings")
	fun getSettings(): Call<Body<Settings>>

	@POST("api/users/saveSettings")
	fun saveSettings(
		@RetrofitBody settings: Settings
	): Call<Body<Any>>

	@GET("api/account-{accountUrl}/moves/summary/{time}")
	fun getSummary(
		@Path("accountUrl") accountUrl: String,
		@Path("time") time: Int
	): Call<Body<Summary>>

	@FormUrlEncoded
	@POST("api/account-{accountUrl}/users/tfa")
	fun validateTFA(
		@Field("code") code: String
	): Call<Body<Any>>

	@GET("api/moves/lists")
	fun listsForMoves(): Call<Body<Lists>>

	@POST("api/users/wipe")
	fun wipe(
		@RetrofitBody wipe: Wipe
	): Call<Body<Any>>

	@GET("api/users/terms")
	fun getTerms(): Call<Body<Terms>>

	@GET("api")
	fun wakeUpSite(): Call<Body<Any>>

	@GET("api/log/count")
	fun countErrors(): Call<Body<ErrorList>>

	@GET("api/log/list")
	fun listErrors(): Call<Body<ErrorList>>

	@POST("api/log/archive/{id}")
	fun archiveErrors(
		@Path("id") id: Int
	): Call<Body<Any>>
}
