package com.dontflymoney.dialogs

import com.dontflymoney.baseactivity.Form
import com.dontflymoney.baseactivity.Message
import com.dontflymoney.entities.Move
import com.dontflymoney.view.R
import com.dontflymoney.viewhelper.DialogSelectClickListener

import org.json.JSONArray
import org.json.JSONException

class DialogAccountOut(list: JSONArray, private val form: Form, private val message: Message, private val move: Move) : DialogSelectClickListener(list) {


    override fun setResult(text: String, value: String) {
        form.setValue(R.id.account_out, text)
        move.AccountOut = value
    }

    override fun handleError(exception: JSONException) {
        message.alertError(R.string.error_convert_result)
    }
}

