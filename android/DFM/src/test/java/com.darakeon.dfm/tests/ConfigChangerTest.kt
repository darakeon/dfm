package com.darakeon.dfm.tests

import android.os.Build
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.not
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class ConfigChangerTest {
	@Test
	fun setAndroidVersionToLollipop() {
		val version = Build.VERSION_CODES.LOLLIPOP

		assertThat(Build.VERSION.SDK_INT, not(`is`(version)))

		setAndroidVersion(version)
		assertThat(Build.VERSION.SDK_INT, `is`(version))
	}
}
