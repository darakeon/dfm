package com.dontflymoney.userdata

import android.app.Activity
import android.content.res.Configuration
import android.content.res.Resources

import com.dontflymoney.baseactivity.SmartActivity
import com.dontflymoney.view.R

import java.util.Locale


object Language {
    private val spKey = "Language"

    fun ChangeAndSave(activity: SmartActivity, language: String) {
        var language = language
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
        change(activity, language)
    }

    private fun change(activity: Activity, language: String) {
        val resources = activity.resources

        val availableLocales = Locale.getAvailableLocales()
        var locale: Locale? = null

        for (availableLocale in availableLocales) {
            if (availableLocale.toString().equals(language, ignoreCase = true)) {
                locale = availableLocale
            }
        }

        if (locale == null)
            return

        Locale.setDefault(locale)

        val config = resources.configuration
        config.setLocale(locale)

        //NEW
        activity.createConfigurationContext(config)

        //OLD
        resources.updateConfiguration(config, null)
        resources.flushLayoutCache()
    }

}