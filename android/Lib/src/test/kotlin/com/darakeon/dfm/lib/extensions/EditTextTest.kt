package com.darakeon.dfm.lib.extensions

import android.widget.EditText
import com.darakeon.dfm.lib.utils.ActivityMock
import com.darakeon.dfm.lib.utils.ApiActivity
import com.darakeon.dfm.testutils.LogRule
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class EditTextTest {
	@get:Rule
	val log = LogRule()

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
