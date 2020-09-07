package com.darakeon.dfm.extensions

import android.view.KeyEvent
import android.widget.EditText
import com.darakeon.dfm.utils.api.ActivityMock
import com.darakeon.dfm.utils.activity.TestActivity
import org.hamcrest.CoreMatchers.equalTo
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class MaskTest {
	private lateinit var activity: TestActivity
	private lateinit var editable: EditText

	@Before
	fun setup() {
		activity = ActivityMock(TestActivity::class).create()
		editable = EditText(activity)
	}

	@Test
	fun useMask() {
		editable.addMask("#.#")
		editable.append("aa")

		assertThat(editable.text.toString(), equalTo("a.a"))
	}

	@Test
	fun addChar() {
		editable.addMask("#.#.#")

		editable.append("a")
		assertThat(editable.text.toString(), equalTo("a."))

		editable.append("b")
		assertThat(editable.text.toString(), equalTo("a.b."))

		editable.append("c")
		assertThat(editable.text.toString(), equalTo("a.b.c"))
	}

	@Test
	fun moreCharsThanAllowed() {
		editable.addMask("#.#")

		editable.append("ab")
		assertThat(editable.text.toString(), equalTo("a.b"))

		editable.append("c")
		assertThat(editable.text.toString(), equalTo("a.b"))
	}

	@Test
	fun removeChar() {
		editable.addMask("#.#")

		editable.append("a.b")
		assertThat(editable.text.toString(), equalTo("a.b"))

		val backspace = KeyEvent(
			KeyEvent.ACTION_DOWN,
			KeyEvent.KEYCODE_DEL
		)
		editable.dispatchKeyEvent(backspace)
		assertThat(editable.text.toString(), equalTo("a."))
	}
}
