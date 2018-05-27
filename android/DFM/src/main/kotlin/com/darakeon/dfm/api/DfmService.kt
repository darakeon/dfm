package com.darakeon.dfm.api

import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.Path

internal interface DfmService {
	@GET("Api-{ticket}/accounts/list")
	fun listAccounts(
		@Path("ticket") ticket: String
	): Call<Body<Array<Account>>>
}
