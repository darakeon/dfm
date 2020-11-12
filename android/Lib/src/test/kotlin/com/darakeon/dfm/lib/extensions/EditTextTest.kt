package com.darakeon.dfm.lib.extensions

import android.widget.EditText
import com.darakeon.dfm.lib.utils.ActivityMock
import com.darakeon.dfm.lib.utils.ApiActivity
import com.darakeon.dfm.testutils.BaseTest
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class EditTextTest: BaseTest() {
	@Test
	fun onChange() {
		val activity = ActivityMock(ApiActivity::class).create()
		val editText = EditText(activity)

		var text = "x"
		editText.onChange { text = it }

		editText.setText("z")

		assertThat(text, `is`("z"))
	}
}
