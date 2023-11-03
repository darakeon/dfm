package com.darakeon.dfm.utils.activity

import android.widget.Button
import androidx.viewbinding.ViewBinding
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.BottomMenuBinding
import com.darakeon.dfm.databinding.WelcomeBinding
import com.darakeon.dfm.lib.api.Api

class TestActivity : TestBaseActivity() {
	var testTitle = 0
	override val title: Int
		get() = testTitle

	var testContentView = 0
	override val contentViewId: Int
		get() = testContentView

	fun testGetExtraOrUrl(key: String) =
		super.getExtraOrUrl(key)

	fun testCallApi(call: (Api<BaseActivity<ViewBinding>>) -> Unit) =
		super.callApi(call)

	fun testDestroy() =
		super.onDestroy()

	private var waitStarted = false
	override fun startWait() {
		super.startWait()
		waitStarted = true
	}

	private var waitEnded = false
	override fun endWait() {
		super.endWait()
		waitEnded = true
	}

	var isBottomMenuTest = false
	private lateinit var welcomeTestBinding: WelcomeBinding
	private lateinit var bottomMenuBinding: BottomMenuBinding
	override fun inflateBinding(): ViewBinding {
		return if (isBottomMenuTest) {
			bottomMenuBinding = BottomMenuBinding.inflate(layoutInflater)
			bottomMenuBinding
		} else {
			welcomeTestBinding = WelcomeBinding.inflate(layoutInflater)
			welcomeTestBinding
		}
	}

	override fun getLogoutButton(): Button {
		return if (isBottomMenuTest)
			bottomMenuBinding.actionLogout
		else
			welcomeTestBinding.actionLogout
	}
}
