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
