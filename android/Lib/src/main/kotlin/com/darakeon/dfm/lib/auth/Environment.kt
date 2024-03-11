package com.darakeon.dfm.lib.auth

import android.app.Activity
import android.content.Context
import android.graphics.Color
import com.darakeon.dfm.lib.R
import com.darakeon.dfm.lib.api.entities.Environment
import com.darakeon.dfm.lib.api.entities.Theme
import com.darakeon.dfm.lib.extensions.getColorByAttr
import com.darakeon.dfm.lib.extensions.refresh
import java.util.Locale

private const val themeKey = "Theme"
private const val languageKey = "Language"

fun Activity.setEnvironment(env: Environment) {
	val changedTheme = themeChangeAndSave(env.theme.enum)
	val changedLanguage = languageChangeAndSave(env.language)

	if (changedTheme || changedLanguage)
		refresh()
}

private fun Context.themeChangeAndSave(theme: Theme): Boolean {
	val themeId = theme.style
	val spTheme = themeId.toString()

	if (getValue(themeKey) == spTheme)
		return false

	setTheme(themeId)

	setValue(themeKey, spTheme)

	return true
}

private fun Context.languageChangeAndSave(systemLanguage: String): Boolean {
	if (systemLanguage == "") return false

	val language = systemLanguage.replace("-", "_")

	if (getValue(languageKey) == language)
		return false

	change(language)

	setValue(languageKey, language)

	return true
}

private fun Context.change(language: String) {
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

fun Context.getThemeLineColor(position: Int): Int {
	return if (position % 2 == 0)
		Color.TRANSPARENT
	else
		getColorByAttr(R.attr.background_highlight)
}
