package com.darakeon.dfm.extensions

import android.view.KeyEvent
import android.widget.EditText
import com.darakeon.dfm.utils.activity.ActivityMock
import org.hamcrest.CoreMatchers.equalTo
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class MaskTest {
	@Test
	fun useMask() {
		val context = ActivityMock().create()
		val editable = EditText(context)

		editable.addMask("#.#")
		editable.append("aa")

		assertThat(editable.text.toString(), equalTo("a.a"))
	}

	@Test
	fun addChar() {
		val context = ActivityMock().create()
		val editable = EditText(context)

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
		val context = ActivityMock().create()
		val editable = EditText(context)

		editable.addMask("#.#")

		editable.append("ab")
		assertThat(editable.text.toString(), equalTo("a.b"))

		editable.append("c")
		assertThat(editable.text.toString(), equalTo("a.b"))
	}

	@Test
	fun removeChar() {
		val context = ActivityMock().create()
		val editable = EditText(context)

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
