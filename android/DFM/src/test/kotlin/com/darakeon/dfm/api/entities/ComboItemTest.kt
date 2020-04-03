package com.darakeon.dfm.api.entities

import android.widget.TextView
import com.darakeon.dfm.utils.execute
import com.darakeon.dfm.utils.log.LogRule
import org.hamcrest.MatcherAssert.assertThat
import org.hamcrest.Matchers.`is`
import org.junit.Rule
import org.junit.Test
import org.mockito.ArgumentMatchers.anyString
import org.mockito.Mockito.`when`
import org.mockito.Mockito.mock

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

		val field = mock(TextView::class.java)

		var text = ""
		`when`(field.setText(anyString()))
			.execute { text = it[0] as String }

		list.setLabel("value2", field)

		assertThat(text, `is`("text2"))
	}
}
