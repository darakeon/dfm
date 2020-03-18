package com.darakeon.dfm.extensions

import android.widget.EditText
import com.darakeon.dfm.utils.activity.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class EditTextTest {
	@Test
	fun onChange() {
		val activity = ActivityMock.create()
		val editText = EditText(activity)

		var text = "x"
		editText.onChange { text = it }

		editText.setText("z")

		assertThat(text, `is`("z"))
	}
}
