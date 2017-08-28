package com.darakeon.dfm.uiHelpers.dialogs

import com.darakeon.dfm.activities.MovesCreateActivity
import com.darakeon.dfm.R
import org.json.JSONArray
import org.json.JSONException

class DialogNature(list: JSONArray?, private val activity: MovesCreateActivity) : DialogSelectClickListener(list) {

	override fun setResult(text: String, value: String) {
		activity.setNature(text, value)
	}

	override fun handleError(exception: JSONException) {
		activity.message.alertError(R.string.error_convert_result)
	}
}
