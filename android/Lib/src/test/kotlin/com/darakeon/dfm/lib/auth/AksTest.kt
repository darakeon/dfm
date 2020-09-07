package com.darakeon.dfm.lib.auth

import android.content.Context
import com.darakeon.dfm.testutils.LogRule
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.not
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Rule
import org.junit.Test
import org.mockito.Mockito.mock

class AksTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun encrypt() {
		val context = mock(Context::class.java)
		val aks = Aks(context)

		val message = "Hey, listen!"
		val encrypted = aks.encrypt(message)

		assertThat(encrypted, not(`is`(message)))
	}

	@Test
	fun decrypt() {
		val context = mock(Context::class.java)
		val aks = Aks(context)

		val message = "Hey, listen!"
		val encrypted = aks.encrypt(message)
		val decrypted = aks.decrypt(encrypted)

		assertThat(decrypted, `is`(message))
	}
}
