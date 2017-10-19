package com.darakeon.dfm.user

import android.app.Activity
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.objects.SmartStatic
import java.util.*

private val spKey = "Language"

fun <T : SmartStatic> SmartActivity<T>.languageChangeAndSave(systemLanguage: String) {
	var language = systemLanguage
	val current = Locale.getDefault().toString()

	language = language.replace("-", "_")

	if (language.equals(current, ignoreCase = true))
		return

	change(language)

	setValue(spKey, language)

	refresh()
}

fun Activity.languageChangeFromSaved() {
	val language = getValue(spKey)

	if (language != "")
		change(language)
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