package com.darakeon.dfm.utils.activity

import android.view.ContextMenu
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.lib.api.Api

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

	override fun changeContextMenu(
		menu: ContextMenu,
		view: View,
		menuInfo: ContextMenu.ContextMenuInfo
	) {
		inflatedContextMenu = true
	}

	fun testGetExtraOrUrl(key: String) =
		super.getExtraOrUrl(key)

	fun testCallApi(call: (Api<BaseActivity>) -> Unit) =
		super.callApi(call)

	fun testDestroy() =
		super.onDestroy()

	var waitEnded = false
	override fun endWait() {
		super.endWait()
		waitEnded = true
	}
}
