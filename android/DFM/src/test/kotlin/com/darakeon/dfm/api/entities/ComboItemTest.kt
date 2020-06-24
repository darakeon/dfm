package com.darakeon.dfm.api.entities

import android.content.Context
import android.widget.AutoCompleteTextView
import android.widget.Button
import android.widget.TextView
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.execute
import com.darakeon.dfm.utils.log.LogRule
import junit.framework.Assert.assertTrue
import org.hamcrest.MatcherAssert.assertThat
import org.hamcrest.Matchers.`is`
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.mockito.ArgumentMatchers.anyString
import org.mockito.Mockito.`when`
import org.mockito.Mockito.mock
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class ComboItemTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun setLabel() {
		val list = arrayOf(
			ComboItem("text1", "value1"),
			ComboItem("text2", "value2"),
			ComboItem("text3", "value3")
		)

		val context = ActivityMock().create()
		val field = TextView(context)

		list.setLabel(field, "value2")

		assertThat(field.text.toString(), `is`("text2"))
	}

	var comboValue: String? = "value2"

	@Test
	fun setCombo() {
		val list = arrayOf(
			ComboItem("text1", "value1"),
			ComboItem("text2", "value2"),
			ComboItem("text3", "value3")
		)

		val context = ActivityMock().create()
		val field = AutoCompleteTextView(context)
		val button = Button(context)

		var opened = false

		list.setCombo(field, button, this::comboValue) { opened = true }
		assertThat(field.text.toString(), `is`("text2"))

		button.performClick()
		assertTrue(opened)

		field.setText("text3")
		assertThat(comboValue, `is`("value3"))
	}
}
