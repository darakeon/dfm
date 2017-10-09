package com.darakeon.dfm.activities.base

import com.darakeon.dfm.R
import com.darakeon.dfm.activities.objects.SmartStatic
import com.darakeon.dfm.api.Step
import com.darakeon.dfm.user.LanguageChangeAndSave
import com.darakeon.dfm.user.ThemeChangeAndSave
import org.json.JSONObject

internal class ResultHandler<T : SmartStatic>(private val activity: SmartActivity<T>) {

	fun HandlePostResult(result: JSONObject, step: Step) {
		if (result.has("error")) {
			val error = result.getString("error")

			activity.alertError(error)

			if (error.contains(activity.getString(R.string.uninvited)) || error.contains("uninvited")) {
				activity.logout()
			}
		} else {
			val data = result.getJSONObject("data")

			if (data.has("Language"))
				activity.LanguageChangeAndSave(data.getString("Language"))

			if (data.has("Theme"))
				activity.ThemeChangeAndSave(data.getString("Theme"))

			activity.HandleSuccess(data, step)
		}
	}

	fun HandlePostError(error: String) {
		activity.alertError(error)
	}


}
