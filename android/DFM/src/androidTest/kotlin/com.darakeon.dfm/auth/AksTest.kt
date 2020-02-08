package com.darakeon.dfm.auth

import android.support.test.InstrumentationRegistry
import android.support.test.runner.AndroidJUnit4
import org.junit.Assert
import org.junit.Test
import org.junit.runner.RunWith

@RunWith(AndroidJUnit4::class)
class AksTest {

	@Test
	fun encryptionAndDecryption() {
		val context = InstrumentationRegistry.getTargetContext()
		Assert.assertNotNull(context)

		val aks = Aks(context)

		val message = "Hey, listen!"

		val cipher = aks.encrypt(message)
		val result = aks.decrypt(cipher)

		Assert.assertNotEquals(message, cipher)
		Assert.assertEquals(message, result)
	}
}
