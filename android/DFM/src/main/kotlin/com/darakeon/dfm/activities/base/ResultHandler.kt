package com.darakeon.dfm.activities.base

import com.darakeon.dfm.R
import com.darakeon.dfm.activities.objects.SmartStatic
import com.darakeon.dfm.api.Step
import com.darakeon.dfm.user.Language
import com.darakeon.dfm.user.Theme
import org.json.JSONObject

internal class ResultHandler<T : SmartStatic>(private val activity: SmartActivity<T>, private val navigation: Navigation<T>) {

	fun HandlePostResult(result: JSONObject, step: Step) {
		if (result.has("error")) {
			val error = result.getString("error")

			activity.message.alertError(error)

			if (error.contains(activity.getString(R.string.uninvited)) || error.contains("uninvited")) {
				navigation.logout()
			}
		} else {
			val data = result.getJSONObject("data")

			if (data.has("Language"))
				Language.ChangeAndSave(activity, data.getString("Language"))

			if (data.has("Theme"))
				Theme.ChangeAndSave(activity, data.getString("Theme"))

			activity.HandleSuccess(data, step)
		}
	}

	fun HandlePostError(error: String) {
		activity.message.alertError(error)
	}


}
