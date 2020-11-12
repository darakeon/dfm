package com.darakeon.dfm.lib.api.entities

import android.app.Activity
import android.widget.AutoCompleteTextView
import android.widget.Button
import android.widget.TextView
import com.darakeon.dfm.testutils.BaseTest
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.Robolectric.buildActivity
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class ComboItemTest: BaseTest() {
	lateinit var activity: Activity

	@Before
	fun setup() {
		activity = buildActivity(
			Activity::class.java
		).create().get()
	}

	@Test
	fun setLabel() {
		val list = arrayOf(
			ComboItem("text1", "value1"),
			ComboItem("text2", "value2"),
			ComboItem("text3", "value3")
		)

		val field = TextView(activity)

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

		val field = AutoCompleteTextView(activity)
		val button = Button(activity)

		var opened = false

		list.setCombo(field, button, this::comboValue) { opened = true }
		assertThat(field.text.toString(), `is`("text2"))

		button.performClick()
		assertTrue(opened)

		field.setText("text3")
		assertThat(comboValue, `is`("value3"))
	}
}
