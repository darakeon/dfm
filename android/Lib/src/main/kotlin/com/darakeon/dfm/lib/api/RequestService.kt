package com.darakeon.dfm.lib.api

import com.darakeon.dfm.lib.api.entities.Body
import com.darakeon.dfm.lib.api.entities.accounts.AccountList
import com.darakeon.dfm.lib.api.entities.errors.ErrorList
import com.darakeon.dfm.lib.api.entities.extract.Extract
import com.darakeon.dfm.lib.api.entities.login.Login
import com.darakeon.dfm.lib.api.entities.login.Ticket
import com.darakeon.dfm.lib.api.entities.moves.Lists
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.lib.api.entities.moves.MoveCreation
import com.darakeon.dfm.lib.api.entities.settings.Settings
import com.darakeon.dfm.lib.api.entities.signup.SignUp
import com.darakeon.dfm.lib.api.entities.status.StatusResponse
import com.darakeon.dfm.lib.api.entities.summary.Summary
import com.darakeon.dfm.lib.api.entities.terms.Terms
import com.darakeon.dfm.lib.api.entities.tfa.TFA
import com.darakeon.dfm.lib.api.entities.toggleCheck.ToggleCheck
import com.darakeon.dfm.lib.api.entities.wipe.Wipe
import retrofit2.Call
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.PATCH
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Path
import retrofit2.http.Query
import java.util.UUID
import retrofit2.http.Body as RetrofitBody

interface RequestService {
	@GET("accounts")
	fun listAccounts(): Call<Body<AccountList>>

	@GET("accounts/{accountUrl}/extract")
	fun getExtract(
		@Path("accountUrl") accountUrl: String,
		@Query("year") year: Short,
		@Query("month") month: Short,
	): Call<Body<Extract>>

	@PATCH("moves/{id}/check")
	fun check(
		@Path("id") id: UUID,
		@RetrofitBody toggleCheck: ToggleCheck
	): Call<Body<Any>>

	@PATCH("moves/{id}/uncheck")
	fun uncheck(
		@Path("id") id: UUID,
		@RetrofitBody toggleCheck: ToggleCheck
	): Call<Body<Any>>

	@DELETE("moves/{id}")
	fun delete(
		@Path("id") id: UUID
	): Call<Body<Any>>

	@POST("users/signup")
	fun signup(
		@RetrofitBody signup: SignUp
	): Call<Body<Any>>

	@POST("users/login")
	fun login(
		@RetrofitBody login: Login
	): Call<Body<Ticket>>

	@PATCH("users/logout")
	fun logout(): Call<Body<Any>>

	@GET("moves/{id}")
	fun getMove(
		@Path("id") id: UUID
	): Call<Body<MoveCreation>>

	@POST("moves")
	fun saveMove(
		@RetrofitBody move: Move
	): Call<Body<Any>>

	@PUT("moves/{id}")
	fun saveMove(
		@Path("id") id: UUID,
		@RetrofitBody move: Move
	): Call<Body<Any>>

	@GET("settings")
	fun getSettings(): Call<Body<Settings>>

	@PATCH("settings")
	fun saveSettings(
		@RetrofitBody settings: Settings
	): Call<Body<Any>>

	@GET("accounts/{accountUrl}/summary")
	fun getSummary(
		@Path("accountUrl") accountUrl: String,
		@Query("year") year: Short,
	): Call<Body<Summary>>

	@PATCH("users/tfa")
	fun validateTFA(
		@RetrofitBody tfa: TFA
	): Call<Body<Any>>

	@GET("moves/relations")
	fun listsForMoves(): Call<Body<Lists>>

	@PATCH("users/wipe")
	fun wipe(
		@RetrofitBody wipe: Wipe
	): Call<Body<Any>>

	@GET("users/terms")
	fun getTerms(): Call<Body<Terms>>

	@GET("/")
	fun wakeUpSite(): Call<Body<StatusResponse>>

	@GET("logs")
	fun listErrors(): Call<Body<ErrorList>>

	@GET("logs/count")
	fun countErrors(): Call<Body<ErrorList>>

	@PATCH("logs/{id}/archive")
	fun archiveErrors(
		@Path("id") id: String
	): Call<Body<Any>>
}
