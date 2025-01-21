package com.darakeon.dfm.testutils.api

import android.os.Build
import androidx.annotation.RequiresApi
import com.darakeon.dfm.testutils.robolectric.Waitable
import okhttp3.mockwebserver.MockResponse
import okhttp3.mockwebserver.MockWebServer
import retrofit2.Retrofit
import kotlin.reflect.KClass

class Server<RS : Any>(
	requestServiceClass: KClass<RS>,
	build: (String) -> Retrofit
) : Waitable {
	private val server: MockWebServer = MockWebServer()
	val service: RS
	val url: String

	override var count: Int = 0
		private set

	init {
		url = server.url("").toString()

		val retrofit = build(url)

		service = retrofit.create(
			requestServiceClass.java
		)
	}

	fun shutdown() {
		server.shutdown()
	}

	@RequiresApi(Build.VERSION_CODES.O)
	fun enqueue(jsonName: String) {
		val jsonBody = readResponse(jsonName)
		val response = MockResponse().setBody(jsonBody)
		server.enqueue(response)
		count++
	}

	@RequiresApi(Build.VERSION_CODES.O)
	fun enqueue(statusCode: Int) {
		val response = MockResponse().setResponseCode(statusCode)
		server.enqueue(response)
		count++
	}

	fun getPath(): String {
		return server.takeRequest().path ?: ""
	}

	fun lastPath(): String {
		var path = ""
		for (i in 1..server.requestCount) {
			path = server.takeRequest().path ?: ""
		}
		return path
	}
}
