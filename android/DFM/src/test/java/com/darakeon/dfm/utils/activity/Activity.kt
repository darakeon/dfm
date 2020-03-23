package com.darakeon.dfm.utils.activity

import android.app.Activity
import android.view.ContextMenu
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.Api
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.utils.api.Server
import org.robolectric.Robolectric.buildActivity

object ActivityMock {
	inline fun <reified T : Activity?> create(): T =
		buildActivity(T::class.java).create().get()

	fun create() = create<TestActivity>()

	inline fun <reified T : Activity?> get(): T =
		buildActivity(T::class.java).get()

	fun get() = get<TestActivity>()
}

class TestActivity : BaseActivity() {
	val server = Server()

	init {
		serverUrl = server.url
	}

	var testTitle = 0
	override val title: Int
		get() = testTitle

	var testContentView = 0
	override val contentView: Int
		get() = testContentView

	override val contextMenuResource: Int
		get() = if (hasContextMenu) R.menu.move_options else 0

	var testViewWithContext: View? = null
	override val viewWithContext: View?
		get() = testViewWithContext

	var hasContextMenu = false
	var inflatedContextMenu = false
		private set

	override fun changeContextMenu(view: View, menuInfo: ContextMenu) {
		inflatedContextMenu = true
	}

	fun testGetExtraOrUrl(key: String) =
		super.getExtraOrUrl(key)

	fun testCallApi(call: (Api) -> Unit) =
		super.callApi(call)

	fun testDestroy() =
		super.onDestroy()
}
