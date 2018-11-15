package com.darakeon.dfm.api

import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.api.entities.accounts.AccountList
import com.darakeon.dfm.api.entities.extract.Extract
import com.darakeon.dfm.api.entities.login.Login
import com.darakeon.dfm.api.entities.moves.MoveCreation
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path

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
		@retrofit2.http.Body login: Login.Request
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
}
