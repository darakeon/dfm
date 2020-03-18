package com.darakeon.dfm.utils.activity

import android.app.Activity
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.utils.api.Server
import org.robolectric.Robolectric.buildActivity

object ActivityMock {
	inline fun <reified T : Activity?> create(): T =
		buildActivity(T::class.java).create().get()

	fun create(): TestActivity = create<TestActivity>()
}

class TestActivity : BaseActivity() {
	val server = Server()

	init {
		serverUrl = server.url
	}

	override val contentView: Int = 0
	override val hasTitle: Boolean
		get() = false
}
