package com.darakeon.dfm.testutils.robolectric

import android.app.Activity
import com.darakeon.dfm.testutils.api.Server
import org.robolectric.Robolectric.buildActivity
import retrofit2.Retrofit
import kotlin.reflect.KClass

open class ActivityMock<RS: Any, Act, Api: Any>(
	requestServiceClass: KClass<RS>,
	build: (String) -> Retrofit,
	private val activityClass: KClass<Act>,
	private val setPrivate: (obj: Any, prop: String, value: Any) -> Unit,
	private val newApi: (Act, String) -> Api
) where Act : Activity {
	val server = Server(requestServiceClass, build)

	fun create(): Act = build(true)
	fun get(): Act = build(false)

	private fun build(create: Boolean): Act {
		val builder = buildActivity(activityClass.java)

		if (create)
			builder.create()

		val activity = builder.get()

		setPrivate(activity, "serverUrl", server.url)

		if (create) {
			setPrivate(activity, "api", newApi(activity, server.url))
		}

		return activity
	}
}
