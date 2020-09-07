package com.darakeon.dfm.lib.tests

import android.os.Build
import com.darakeon.dfm.testutils.setAndroidVersion
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.CoreMatchers.not
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class ConfigChangerAndroidTest {
	@Test
	fun setAndroidVersionToMarshmallow() {
		val version = Build.VERSION_CODES.M

		assertThat(Build.VERSION.SDK_INT, not(`is`(version)))

		setAndroidVersion(version)
		assertThat(Build.VERSION.SDK_INT, `is`(version))
	}
}
