package com.darakeon.dfm.auth

import android.support.test.InstrumentationRegistry
import org.junit.Assert
import org.junit.Test

class AksAndroidTest {
	@Test
	fun encryptionAndDecryption_23AndSoOn() {
		val aks = Aks()

		val message = "Hey, listen!"

		val cipher = aks.encrypt(message)
		val result = aks.decrypt(cipher)

		Assert.assertNotEquals(message, cipher)
		Assert.assertEquals(message, result)
	}

	@Test
	fun encryptionAndDecryption_below23() {
		val context = InstrumentationRegistry.getTargetContext()
		val aks = OldAks(context)

		val message = "Hey, listen!"

		val cipher = aks.encrypt(message)
		val result = aks.decrypt(cipher)

		Assert.assertNotEquals(message, cipher)
		Assert.assertEquals(message, result)
	}
}
