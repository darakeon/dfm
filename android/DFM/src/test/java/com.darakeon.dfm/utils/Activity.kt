package com.darakeon.dfm.utils

import android.app.Activity
import com.darakeon.dfm.base.BaseActivity
import org.robolectric.Robolectric.buildActivity

object ActivityMock {
	inline fun <reified T : Activity?> create(): T =
		buildActivity(T::class.java).create().get()

	fun create(): BaseActivity = create<TestActivity>()
}

class TestActivity : BaseActivity(

) {
	override val contentView: Int = 0
	override val hasTitle: Boolean
		get() = false
}
