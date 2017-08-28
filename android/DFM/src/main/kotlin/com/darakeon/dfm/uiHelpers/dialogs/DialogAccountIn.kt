package com.darakeon.dfm.uiHelpers.dialogs

import com.darakeon.dfm.activities.base.Form
import com.darakeon.dfm.activities.base.Message
import com.darakeon.dfm.activities.objects.SmartStatic
import com.darakeon.dfm.api.entities.Move
import com.darakeon.dfm.R
import org.json.JSONArray
import org.json.JSONException

class DialogAccountIn<T : SmartStatic>(list: JSONArray?, private val form: Form, private val message: Message<T>, private val move: Move?) : DialogSelectClickListener(list) {


    override fun setResult(text: String, value: String) {
        form.setValue(R.id.account_in, text)
        move?.AccountIn = value
    }

    override fun handleError(exception: JSONException) {
        message.alertError(R.string.error_convert_result)
    }
}
