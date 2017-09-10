package com.darakeon.dfm.uiHelpers.dialogs

import android.app.Activity
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.Message
import com.darakeon.dfm.activities.base.setValue
import com.darakeon.dfm.activities.objects.MovesCreateStatic
import com.darakeon.dfm.api.entities.Move
import org.json.JSONArray
import org.json.JSONException

class DialogCategory(list: JSONArray?, private val activity: Activity, private val message: Message<MovesCreateStatic>, private val move: Move?) : DialogSelectClickListener(list) {

	override fun setResult(text: String, value: String) {
		activity.setValue(R.id.category, text)
		move?.Category = value
	}

	override fun handleError(exception: JSONException) {
		message.alertError(R.string.error_convert_result)
	}
}
