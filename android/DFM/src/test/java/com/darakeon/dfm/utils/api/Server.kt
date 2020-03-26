package com.darakeon.dfm.utils.api

import com.darakeon.dfm.api.RequestService
import com.darakeon.dfm.api.Retrofit
import okhttp3.mockwebserver.MockResponse
import okhttp3.mockwebserver.MockWebServer

class Server {
	private val server: MockWebServer = MockWebServer()
	internal val service: RequestService
	internal val url: String

	init {
		url = server.url("").toString()

		val retrofit = Retrofit.build(url)

		service = retrofit.create(
			RequestService::class.java
		)
	}

	fun shutdown() {
		server.shutdown()
	}

	fun enqueue(jsonName: String) {
		val jsonBody = readResponse(jsonName)
		val response = MockResponse().setBody(jsonBody)
		server.enqueue(response)
	}
}
