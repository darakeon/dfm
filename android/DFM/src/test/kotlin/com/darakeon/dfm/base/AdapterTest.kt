package com.darakeon.dfm.base

import android.R
import android.app.Activity
import android.graphics.Color
import android.view.LayoutInflater
import android.view.View
import android.widget.TextView
import com.darakeon.dfm.api.entities.Environment
import com.darakeon.dfm.auth.lighter1
import com.darakeon.dfm.auth.setEnvironment
import com.darakeon.dfm.utils.activity.MockContext
import com.darakeon.dfm.utils.execute
import com.darakeon.dfm.utils.log.LogRule
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNotNull
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.mockito.ArgumentMatchers.anyString
import org.mockito.Mockito.`when`
import org.mockito.Mockito.anyInt
import org.mockito.Mockito.mock

class AdapterTest {
	@get:Rule
	val log = LogRule()

	private lateinit var activity: Activity
	private lateinit var inflater: LayoutInflater
	private lateinit var adapter: MockAdapter

	@Before
	fun setup() {
		val mocker = MockContext()
			.mockSharedPreferences()
			.mockResources()

		activity = mocker.activity
		activity.setEnvironment(Environment("Dark", ""))

		inflater = mocker.mockInflater()

		val list = listOf("L", "C", "S", "D")
		adapter = MockAdapter(activity, list)
	}

	@Test
	fun getCount() {
		assertThat(adapter.count, `is`(4))
	}

	@Test
	fun getItem() {
		assertThat(adapter.getItem(2).toString(), `is`("S"))
	}

	@Test
	fun getItemId() {
		assertThat(adapter.getItemId(3), `is`(3L))
	}

	@Test
	fun getView() {
		val parent = mock(View::class.java)

		val child = mock(TextView::class.java)
		`when`(parent.findViewById<TextView>(R.id.text1))
			.thenReturn(child)

		`when`(inflater.inflate(R.layout.simple_list_item_1, null, false))
			.thenReturn(parent)

		var text = ""
		`when`(child.setText(anyString())).execute {
			text = it[0].toString()
		}
		`when`(child.text).thenAnswer { text }

		var color = 1
		`when`(parent.setBackgroundColor(anyInt()))
			.execute { color = it[0] as Int }

		getView(0, Color.TRANSPARENT, "L", child) { color }
		getView(1, lighter1, "C", child) { color }
		getView(2, Color.TRANSPARENT, "S", child) { color }
		getView(3, lighter1, "D", child) { color }
	}

	private fun getView(
		position: Int,
		expectedColor: Int,
		expectedText: String,
		child: TextView,
		resultColor: () -> Int
	) {
		val result = adapter.getView(position, null, null)

		assertNotNull(result)
		assertThat(resultColor(), `is`(expectedColor))
		assertThat(child.text?.toString(), `is`(expectedText))
	}
}
