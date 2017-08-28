package com.darakeon.dfm.user

import android.app.Activity
import android.graphics.Color
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.objects.SmartStatic

object Theme {
    private val spKey = "Theme"
    private var currentTheme = R.style.AppTheme

    fun <T : SmartStatic> ChangeAndSave(activity: SmartActivity<T>, systemTheme: String) {
        val theme = getRes(systemTheme)

        if (theme == currentTheme)
            return

        change(activity, theme)

        SP.setValue(activity, spKey, theme.toString())

        activity.refresh()
    }

    fun ChangeFromSaved(activity: Activity) {
        val theme = SP.getValue(activity, Theme.spKey)

        if (theme != null)
            Theme.change(activity, theme.toInt())
    }

    private fun change(activity: Activity, theme: Int) {
        activity.setTheme(theme)
        currentTheme = theme;
    }

    private fun getRes(theme: String): Int {
        when (theme) {
            "Light" -> return android.R.style.Theme_Holo_Light
            else -> return android.R.style.Theme_Holo
        }
    }

    fun getLineColor(position: Int): Int {
        if (position % 2 == 0) {
            return Color.TRANSPARENT
        }
        else {
            when (currentTheme) {
                android.R.style.Theme_Holo_Light -> return Color.LTGRAY
                else -> return Color.rgb(0x17, 0x1b, 0x20)
            }
        }
    }

}