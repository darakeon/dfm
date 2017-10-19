package com.darakeon.dfm.uiHelpers.dialogs

import android.app.Activity
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.setValue
import com.darakeon.dfm.api.entities.Move
import org.json.JSONArray

class DialogCategory(list: JSONArray?, private val activity: Activity, private val move: Move?) : DialogSelectClickListener(list) {
	override fun setResult(text: String, value: String) {
		activity.setValue(R.id.category, text)
		move?.category = value
	}
}
