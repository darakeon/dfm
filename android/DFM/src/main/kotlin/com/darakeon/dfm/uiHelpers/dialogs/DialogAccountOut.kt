package com.darakeon.dfm.uiHelpers.dialogs

import android.app.Activity
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.Message
import com.darakeon.dfm.activities.base.setValue
import com.darakeon.dfm.activities.objects.SmartStatic
import com.darakeon.dfm.api.entities.Move
import org.json.JSONArray
import org.json.JSONException

class DialogAccountOut<T : SmartStatic>(list: JSONArray?, private val activity: Activity, private val message: Message<T>, private val move: Move?) : DialogSelectClickListener(list) {

	override fun setResult(text: String, value: String) {
		activity.setValue(R.id.account_out, text)
		move?.AccountOut = value
	}

	override fun handleError(exception: JSONException) {
		message.alertError(R.string.error_convert_result)
	}
}

