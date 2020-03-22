package com.darakeon.dfm.api

import android.content.Context
import android.content.pm.PackageInfo
import android.content.pm.PackageManager
import com.darakeon.dfm.BuildConfig
import com.darakeon.dfm.utils.activity.MockContext
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test
import org.mockito.Mockito.`when`
import org.mockito.Mockito.mock

class MainInfoTest {
	@Test
	fun getSiteUrl() {
		val context = MockContext()
			.mockResources()
			.addStringResource("local_address", "site")
			.activity

		val url = MainInfo.getSiteUrl(context)

		if (BuildConfig.DEBUG)
			assertThat(url, `is`("http://site/"))
		else
			assertThat(url, `is`("https://dontflymoney.com/"))
	}

	@Test
	fun getAppVersion() {
		val context = mock(Context::class.java)

		val manager = mock(PackageManager::class.java)
		`when`(context.packageManager).thenReturn(manager)

		val info = mock(PackageInfo::class.java)
		info.versionName = "ve.r.si.on"
		`when`(manager.getPackageInfo(
			context.packageName, 0
		)).thenReturn(info)

		val version = MainInfo.getAppVersion(context)

		assertThat(version, `is`("ve.r.si.on"))
	}
}
