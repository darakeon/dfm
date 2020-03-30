package com.darakeon.dfm.api.entities

import android.widget.TextView

data class ComboItem(
	val text: String,
	val value: String
)

fun Array<ComboItem>.setLabel(value: String?, field: TextView) {
	val saved = this.firstOrNull {
		it.value == value
	} ?: return

	field.text = saved.text
}

