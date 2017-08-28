package com.darakeon.dfm.uiHelpers.dialogs

import android.content.DialogInterface

import org.json.JSONArray
import org.json.JSONException


abstract class DialogSelectClickListener(private val list: JSONArray?) : DialogInterface.OnClickListener {

    override fun onClick(dialog: DialogInterface, which: Int) {
        try {
            if (list != null) {
                setResult(
                    list.getJSONObject(which).getString("Text"),
                    list.getJSONObject(which).getString("Value")
                )
            }
        } catch (e: JSONException) {
            handleError(e)
        }

    }

    abstract fun setResult(text: String, value: String)

    abstract fun handleError(exception: JSONException)

}