package com.darakeon.dfm.lib.api

import android.content.Context
import android.content.pm.PackageInfo
import android.content.pm.PackageManager
import com.darakeon.dfm.lib.BuildConfig
import com.darakeon.dfm.lib.R
import com.darakeon.dfm.lib.utils.mockContext
import com.darakeon.dfm.testutils.BaseTest
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test
import org.mockito.Mockito.mock
import org.mockito.Mockito.`when`

class MainInfoTest: BaseTest() {
	@Test
	fun getSiteUrlDebug() {
		if (!BuildConfig.DEBUG)
			return

		val context = mockContext()
			.mockResources()
			.addStringResource(R.string.site_address, "127.0.0.1")
			.activity

		val url = MainInfo.getSiteUrl(context)

		assertThat(url, `is`("http://127.0.0.1:2312/"))
	}

	@Test
	fun getSiteUrlRelease() {
		if (BuildConfig.DEBUG)
			return

		val context = mockContext()
			.mockResources()
			.addStringResource(R.string.site_address, "site")
			.activity

		val url = MainInfo.getSiteUrl(context)

		assertThat(url, `is`("https://site/"))
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
