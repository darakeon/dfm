package com.darakeon.dfm.uiHelpers.dialogs

import android.content.DialogInterface

import org.json.JSONArray

abstract class DialogSelectClickListener(private val list: JSONArray?) : DialogInterface.OnClickListener {

	override fun onClick(dialog: DialogInterface, which: Int) {
		if (list != null) {
			setResult(
				list.getJSONObject(which).getString("Text"),
				list.getJSONObject(which).getString("Value")
			)
		}
	}

	abstract fun setResult(text: String, value: String)
}