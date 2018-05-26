package com.darakeon.dfm.auth

import android.app.Activity
import android.graphics.Color
import com.darakeon.dfm.R
import com.darakeon.dfm.base.SmartActivity
import com.darakeon.dfm.base.SmartStatic

private val spKey = "Theme"
private var currentTheme = R.style.AppTheme

fun <T : SmartStatic> SmartActivity<T>.themeChangeAndSave(systemTheme: String) {
	val theme = getRes(systemTheme)

	if (theme == currentTheme)
		return

	changeTheme(theme)

	setValue(spKey, theme.toString())

	refresh()
}

fun Activity.themeChangeFromSaved() {
	val theme = getValue(spKey)

	if (theme != "")
		changeTheme(theme.toInt())
}

private fun Activity.changeTheme(theme: Int) {
	setTheme(theme)
	currentTheme = theme
}

private fun getRes(theme: String): Int =
	when (theme) {
		"Light" -> R.style.Light
		"Dark" -> R.style.Dark
		else -> R.style.AppTheme
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


fun getHighLightColor(): Int =
	when (currentTheme) {
		R.style.Light -> Color.argb(0x22, 0x00, 0x00, 0x00)
		R.style.Dark -> Color.argb(0x22, 0xFF, 0xFF, 0xFF)
		else -> 0
	}
