package com.darakeon.dfm.auth

import android.app.Activity
import com.darakeon.dfm.api.entities.Environment
import com.darakeon.dfm.extensions.refresh

fun Activity.setEnvironment(env: Environment) {
	val changedTheme = themeChangeAndSave(env.mobileTheme)
	val changedLanguage = languageChangeAndSave(env.language)

	if (changedTheme || changedLanguage)
		refresh()
}
