package com.darakeon.dfm.auth

import android.support.test.InstrumentationRegistry
import org.hamcrest.Matchers.`is`
import org.hamcrest.Matchers.not
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class AksAndroidTest {
	@Test
	fun encryptionAndDecryption_23AndSoOn() {
		val aks = Aks()

		val message = "Hey, listen!"

		val cipher = aks.encrypt(message)
		val result = aks.decrypt(cipher)

		assertThat(cipher, not(`is`(message)))
		assertThat(result, `is`(message))
	}

	@Test
	fun encryptionAndDecryption_below23() {
		val context = InstrumentationRegistry.getTargetContext()
		val aks = OldAks(context)

		val message = "Hey, listen!"

		val cipher = aks.encrypt(message)
		val result = aks.decrypt(cipher)

		assertThat(cipher, not(`is`(message)))
		assertThat(result, `is`(message))
	}
}
