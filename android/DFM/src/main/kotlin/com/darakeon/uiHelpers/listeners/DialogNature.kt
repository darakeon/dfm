package com.dontflymoney.listeners

import com.dontflymoney.view.MovesCreateActivity
import com.dontflymoney.view.R
import com.dontflymoney.viewhelper.DialogSelectClickListener

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
