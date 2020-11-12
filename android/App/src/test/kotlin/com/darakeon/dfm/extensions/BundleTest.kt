package com.darakeon.dfm.extensions

import android.os.Bundle
import com.darakeon.dfm.lib.api.entities.Date
import com.darakeon.dfm.testutils.BaseTest
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.After
import org.junit.Before
import org.junit.Test
import org.mockito.Mockito.`when`
import org.mockito.Mockito.anyString
import org.mockito.Mockito.mock

class BundleTest: BaseTest() {
	private lateinit var bundle: Bundle
	private val dic = HashMap<String, String>()

	@Before
	fun setup() {
		bundle = mock(Bundle::class.java)

		`when`(bundle.putCharSequence(anyString(), anyString()))
			.then {
				val key = it.arguments[0].toString()
				val value = it.arguments[1].toString()
				dic.put(key, value)
			}

		`when`(bundle.get(anyString()))
			.then {
				val key = it.arguments[0].toString()
				if (dic.containsKey(key))
					dic[it.arguments[0].toString()]
				else
					null
			}
	}

	@After
	fun tearDown() {
		dic.clear()
	}

	@Test
	fun putAndGetText() {
		bundle.putJson("test", "bundle")
		assertThat(bundle.getFromJson("test", ""), `is`("bundle"))
	}

	@Test
	fun putAndGetObject() {
		val date = Date(2019, 5, 4)

		bundle.putJson("test", date)

		assertThat(bundle.getFromJson("test", Date()), `is`(date))
	}

	@Test
	fun putAndGetDefault() {
		assertThat(bundle.getFromJson("test", Date()), `is`(Date()))
	}
}
