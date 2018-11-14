package com.darakeon.dfm.api

import com.darakeon.dfm.api.entities.AccountList
import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.api.entities.Extract
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

	@POST("Api-{ticket}/Account-{accountUrl}/moves/check/{id}")
	fun check(
		@Path("ticket") ticket: String,
		@Path("accountUrl") accountUrl: String,
		@Path("id") id: Int
	): Call<Body<Any>>

	@POST("Api-{ticket}/Account-{accountUrl}/moves/uncheck/{id}")
	fun uncheck(
		@Path("ticket") ticket: String,
		@Path("accountUrl") accountUrl: String,
		@Path("id") id: Int
	): Call<Body<Any>>

	@POST("Api-{ticket}/Account-{accountUrl}/moves/delete/{id}")
	fun delete(
		@Path("ticket") ticket: String,
		@Path("accountUrl") accountUrl: String,
		@Path("id") id: Int
	): Call<Body<Any>>
}
