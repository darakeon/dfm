package com.darakeon.dfm.user

import android.app.Activity
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.objects.SmartStatic
import java.util.*

object Language {
    private val spKey = "Language"

    fun <T : SmartStatic> ChangeAndSave(activity: SmartActivity<T>, systemLanguage: String) {
        var language = systemLanguage
        val current = Locale.getDefault().toString()

        language = language.replace("-", "_")

        if (language.equals(current, ignoreCase = true))
            return

        change(activity, language)

        SP.setValue(activity, spKey, language)

        activity.refresh()
    }

    fun ChangeFromSaved(activity: Activity) {
        val language = SP.getValue(activity, spKey)

        if (language != null)
            change(activity, language)
    }

    private fun change(activity: Activity, language: String) {
        val resources = activity.resources

        val availableLocales = Locale.getAvailableLocales()

        val locale: Locale = availableLocales.lastOrNull {
            it.toString().equals(language, ignoreCase = true)
        } ?: return

        Locale.setDefault(locale)

        val config = resources.configuration
        config.setLocale(locale)

        //NEW
        activity.createConfigurationContext(config)

        //OLD
        @Suppress("DEPRECATION")
        resources.updateConfiguration(config, null)
        resources.flushLayoutCache()
    }

}