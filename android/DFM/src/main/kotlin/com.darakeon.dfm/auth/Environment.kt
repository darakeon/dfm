package com.darakeon.dfm.auth

import android.app.Activity
import android.content.Context
import android.graphics.Color
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Environment
import com.darakeon.dfm.extensions.refresh
import java.util.Locale

private const val themeKey = "Theme"
private const val languageKey = "Language"

const val light = R.style.Light
const val dark = R.style.Dark

const val lighter1 = 0x11FFFFFF
const val lighter2 = 0x22FFFFFF
const val darker1 = 0x11000000
const val darker2 = 0x22000000

fun Activity.setEnvironment(env: Environment) {
	val changedTheme = themeChangeAndSave(env.mobileTheme)
	val changedLanguage = languageChangeAndSave(env.language)

	if (changedTheme || changedLanguage)
		refresh()
}

private fun Activity.themeChangeAndSave(theme: String): Boolean {
	val themeId = getThemeId(theme)
	val spTheme = themeId.toString()

	if (getValue(themeKey) == spTheme)
		return false

	setTheme(themeId)

	setValue(themeKey, spTheme)

	return true
}

private fun getThemeId(theme: String): Int =
	when (theme) {
		"Light" -> light
		"Dark" -> dark
		else -> R.style.AppTheme
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
	val themeId = getValue(themeKey)

	if (themeId != "")
		setTheme(themeId.toInt())
}

private fun Activity.languageChangeFromSaved() {
	val language = getValue(languageKey)

	if (language != "")
		change(language)
}

private val colors = mapOf(
	Pair(light, arrayOf(Color.TRANSPARENT, darker1, darker2)),
	Pair(dark, arrayOf(Color.TRANSPARENT, lighter1, lighter2))
)

private fun Context.getColors(): Array<Int> {
	val themeId = getValue(themeKey).toIntOrNull()

	return colors[themeId] ?:
		throw NotImplementedError()
}

fun Context.getThemeLineColor(position: Int) =
	getColors()[position % 2]

val Context.highLightColor
	get() = getColors()[2]
