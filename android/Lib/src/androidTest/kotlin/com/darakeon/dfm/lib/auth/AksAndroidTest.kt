package com.darakeon.dfm.lib.auth

import android.os.Build
import android.support.test.InstrumentationRegistry
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.setAndroidVersion
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.not
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class AksAndroidTest: BaseTest() {
	@Test
	fun encryptionAndDecryption_23AndSoOn() {
		setAndroidVersion(Build.VERSION_CODES.M)

		val context = InstrumentationRegistry.getTargetContext()
		val aks = Aks(context)

		val message = "Hey, listen!"

		val cipher = aks.encrypt(message)
		val result = aks.decrypt(cipher)

		assertThat(cipher, not(`is`(message)))
		assertThat(result, `is`(message))
	}

	@Test
	fun encryptionAndDecryption_below23() {
		setAndroidVersion(Build.VERSION_CODES.LOLLIPOP)

		val context = InstrumentationRegistry.getTargetContext()
		val aks = Aks(context)

		val message = "Hey, listen!"

		val cipher = aks.encrypt(message)
		val result = aks.decrypt(cipher)

		assertThat(cipher, not(`is`(message)))
		assertThat(result, `is`(message))
	}
}
