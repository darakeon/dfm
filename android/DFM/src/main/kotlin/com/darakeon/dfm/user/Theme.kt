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

		if (theme != "")
			Theme.change(activity, theme.toInt())
	}

	private fun change(activity: Activity, theme: Int) {
		activity.setTheme(theme)
		currentTheme = theme;
	}

	private fun getRes(theme: String): Int {
		when (theme) {
			"Light" -> return R.style.Light
			"Dark" -> return R.style.Dark
			else -> return R.style.AppTheme
		}
	}

	fun getLineColor(position: Int): Int {
		if (position % 2 == 0) {
			return Color.TRANSPARENT
		}
		else {
			when (currentTheme) {
				R.style.Light -> return Color.argb(0x11, 0x00, 0x00, 0x00)
				R.style.Dark -> return Color.argb(0x11, 0xFF, 0xFF, 0xFF)
				else -> return 0
			}
		}
	}

	fun getHighLightColor(): Int {
		when (currentTheme) {
			R.style.Light -> return Color.argb(0x22, 0x00, 0x00, 0x00)
			R.style.Dark -> return Color.argb(0x22, 0xFF, 0xFF, 0xFF)
			else -> return 0
		}
	}

}