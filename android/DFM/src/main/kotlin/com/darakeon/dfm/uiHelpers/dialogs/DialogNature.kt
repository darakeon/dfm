package com.darakeon.dfm.uiHelpers.dialogs

import com.darakeon.dfm.activities.MovesCreateActivity
import com.darakeon.dfm.R
import org.json.JSONArray

class DialogNature(list: JSONArray?, private val activity: MovesCreateActivity) : DialogSelectClickListener(list) {
	override fun setResult(text: String, value: String) {
		activity.setNature(text, value)
	}
}
