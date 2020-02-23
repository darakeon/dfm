package com.darakeon.dfm.auth

import android.app.Activity
import android.graphics.Color
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Environment
import com.darakeon.dfm.extensions.refresh
import java.util.Locale

private const val themeKey = "Theme"
private const val languageKey = "Language"
private var currentTheme = R.style.AppTheme

fun Activity.setEnvironment(env: Environment) {
	val changedTheme = themeChangeAndSave(env.mobileTheme)
	val changedLanguage = languageChangeAndSave(env.language)

	if (changedTheme || changedLanguage)
		refresh()
}

private fun Activity.themeChangeAndSave(systemTheme: String): Boolean {
	val theme = getRes(systemTheme)

	if (theme == currentTheme)
		return false

	changeTheme(theme)

	setValue(themeKey, theme.toString())

	return true
}

private fun getRes(theme: String): Int =
	when (theme) {
		"Light" -> R.style.Light
		"Dark" -> R.style.Dark
		else -> R.style.AppTheme
	}

private fun Activity.changeTheme(theme: Int) {
	setTheme(theme)
	currentTheme = theme
}

private fun Activity.languageChangeAndSave(systemLanguage: String): Boolean {
	var language = systemLanguage
	val current = Locale.getDefault().toString()

	language = language.replace("-", "_")

	if (language.equals(current, ignoreCase = true))
		return false

	change(language)

	setValue(languageKey, language)

	return true
}

private fun Activity.change(language: String) {
	val availableLocales = Locale.getAvailableLocales()

	val locale: Locale = availableLocales.lastOrNull {
		it.toString().equals(language, ignoreCase = true)
	} ?: return

	Locale.setDefault(locale)

	val config = resources.configuration
	config.setLocale(locale)

	//NEW
	createConfigurationContext(config)

	//OLD
	@Suppress("DEPRECATION")
	resources.updateConfiguration(config, null)
	resources.flushLayoutCache()
}

fun Activity.recoverEnvironment() {
	themeChangeFromSaved()
	languageChangeFromSaved()
}

private fun Activity.themeChangeFromSaved() {
	val theme = getValue(themeKey)

	if (theme != "")
		changeTheme(theme.toInt())
}

private fun Activity.languageChangeFromSaved() {
	val language = getValue(languageKey)

	if (language != "")
		change(language)
}

fun getThemeLineColor(position: Int): Int =
	if (position % 2 == 0) {
		Color.TRANSPARENT
	} else {
		when (currentTheme) {
			R.style.Light -> Color.argb(0x11, 0x00, 0x00, 0x00)
			R.style.Dark -> Color.argb(0x11, 0xFF, 0xFF, 0xFF)
			else -> 0
		}
	}

val highLightColor =
	when (currentTheme) {
		R.style.Light -> Color.argb(0x22, 0x00, 0x00, 0x00)
		R.style.Dark -> Color.argb(0x22, 0xFF, 0xFF, 0xFF)
		else -> 0
	}
