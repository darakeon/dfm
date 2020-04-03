package com.darakeon.dfm.utils.activity

import android.view.ContextMenu
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.Api
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.setPrivate
import com.darakeon.dfm.utils.api.Server
import org.robolectric.Robolectric.buildActivity

class ActivityMock {
	val server = Server()

	internal inline fun <reified T : BaseActivity> create(): T = build(true)
	fun create() = create<TestActivity>()

	internal inline fun <reified T : BaseActivity> get(): T = build(false)
	fun get() = get<TestActivity>()

	private inline fun <reified T : BaseActivity> build(create: Boolean): T {
		val builder = buildActivity(T::class.java)

		if (create)
			builder.create()

		val activity = builder.get()

		activity.setPrivate("serverUrl") {server.url}

		if (create)
			activity.setPrivate("api") { Api(activity) }

		return activity
	}
}

class TestActivity : BaseActivity() {
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

	override fun changeContextMenu(view: View, menu: ContextMenu) {
		inflatedContextMenu = true
	}

	fun testGetExtraOrUrl(key: String) =
		super.getExtraOrUrl(key)

	fun testCallApi(call: (Api) -> Unit) =
		super.callApi(call)

	fun testDestroy() =
		super.onDestroy()
}
