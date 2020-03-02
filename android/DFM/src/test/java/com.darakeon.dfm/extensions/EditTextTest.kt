package com.darakeon.dfm.extensions

import android.widget.EditText
import com.darakeon.dfm.welcome.WelcomeActivity
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.Robolectric.buildActivity
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class EditTextTest {
	@Test
	fun onChange() {
		val act = buildActivity(WelcomeActivity::class.java)
		val editText = EditText(act.create().get())

		var text = "x"
		editText.onChange { text = it }

		editText.setText("z")

		assertThat(text, `is`("z"))
	}
}
