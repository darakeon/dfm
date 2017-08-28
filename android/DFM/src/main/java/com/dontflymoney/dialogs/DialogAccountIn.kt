package com.dontflymoney.dialogs

import com.dontflymoney.activityObjects.SmartStatic
import com.dontflymoney.baseactivity.Form
import com.dontflymoney.baseactivity.Message
import com.dontflymoney.entities.Move
import com.dontflymoney.view.R
import com.dontflymoney.viewhelper.DialogSelectClickListener

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
