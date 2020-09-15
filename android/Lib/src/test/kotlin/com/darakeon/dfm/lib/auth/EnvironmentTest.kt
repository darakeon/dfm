package com.darakeon.dfm.lib.auth

import android.app.Activity
import android.graphics.Color
import com.darakeon.dfm.lib.R
import com.darakeon.dfm.lib.api.entities.Environment
import com.darakeon.dfm.lib.extensions.refresh
import com.darakeon.dfm.lib.utils.mockContext
import com.darakeon.dfm.testutils.LogRule
import com.darakeon.dfm.testutils.execute
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.mockito.ArgumentMatchers.any
import org.mockito.ArgumentMatchers.anyInt
import org.mockito.Mockito.`when`
import java.util.Locale

class EnvironmentTest {
	@get:Rule
	val log = LogRule()

	private lateinit var activity: Activity
	private var refreshed = false

	@Before
	fun setup() {
		val mockContext = mockContext()
			.mockSharedPreferences()
			.mockResources()

		activity = mockContext.activity

		`when`(activity.refresh()).execute { refreshed = true }
	}

	@Test
	fun setEnvironmentWithTheme() {
		var theme = 0
		`when`(activity.setTheme(anyInt())).execute {
			theme = it[0] as Int
		}

		activity.setValue("Theme", "")

		activity.setEnvironment(Environment("Dark", ""))

		assertThat(activity.getValue("Theme"), `is`(R.style.Dark.toString()))
		assertThat(theme, `is`(R.style.Dark))
		assertTrue(refreshed)
	}

	@Test
	fun setEnvironmentWithLanguage() {
		var locale = Locale("")
		val config = activity.resources.configuration
		`when`(config.setLocale(any())).execute {
			locale = it[0] as Locale
		}

		activity.setEnvironment(Environment("", "pt-BR"))

		assertThat(activity.getValue("Language"), `is`("pt_BR"))
		assertThat(locale.displayLanguage, `is`("português"))
		assertTrue(refreshed)
	}

	@Test
	fun recoverEnvironmentWithTheme() {
		var theme = 0
		`when`(activity.setTheme(anyInt())).execute {
			theme = it[0] as Int
		}

		activity.setValue("Theme", R.style.Dark.toString())

		activity.recoverEnvironment()

		assertThat(theme, `is`(R.style.Dark))
	}

	@Test
	fun recoverEnvironmentWithLanguage() {
		var locale = Locale("")

		val config = activity.resources.configuration
		`when`(config.setLocale(any())).execute {
			locale = it[0] as Locale
		}

		activity.setValue("Language", "pt_BR")

		activity.recoverEnvironment()

		assertThat(locale.displayLanguage, `is`("português"))
	}

	@Test
	fun getThemeLineColorEven_Dark() {
		val env = Environment("Dark", "")
		activity.setEnvironment(env)

		assertThat(
			activity.getThemeLineColor(0),
			`is`(Color.TRANSPARENT)
		)
	}

	@Test
	fun getThemeLineColorOdd_Dark() {
		val env = Environment("Dark", "")
		activity.setEnvironment(env)

		assertThat(
			activity.getThemeLineColor(1),
			`is`(lighter1)
		)
	}

	@Test
	fun highLightColor_Dark() {
		val env = Environment("Dark", "")
		activity.setEnvironment(env)

		assertThat(
			activity.highLightColor,
			`is`(lighter2)
		)
	}

	@Test
	fun getThemeLineColorEven_Light() {
		val env = Environment("Light", "")
		activity.setEnvironment(env)

		assertThat(
			activity.getThemeLineColor(0),
			`is`(Color.TRANSPARENT)
		)
	}

	@Test
	fun getThemeLineColorOdd_Light() {
		val env = Environment("Light", "")
		activity.setEnvironment(env)

		assertThat(
			activity.getThemeLineColor(1),
			`is`(darker1)
		)
	}

	@Test
	fun highLightColor_Light() {
		val env = Environment("Light", "")
		activity.setEnvironment(env)

		assertThat(
			activity.highLightColor,
			`is`(darker2)
		)
	}

	@Test(expected = NotImplementedError::class)
	fun getThemeLineColorEven_None() {
		activity.getThemeLineColor(0)
	}

	@Test(expected = NotImplementedError::class)
	fun getThemeLineColorOdd_None() {
		activity.getThemeLineColor(1)
	}

	@Test(expected = NotImplementedError::class)
	fun highLightColor_None() {
		activity.highLightColor
	}
}
