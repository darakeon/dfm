package com.darakeon.dfm.api.entities

import android.widget.AutoCompleteTextView
import android.widget.Button
import android.widget.TextView
import com.darakeon.dfm.extensions.complete
import kotlin.reflect.KMutableProperty0

data class ComboItem(
	val text: String,
	val value: String
)

fun Array<ComboItem>.setLabel(field: TextView, value: String?) {
	val saved = this.firstOrNull {
		it.value == value
	} ?: return

	field.text = saved.text
}

fun Array<ComboItem>.setCombo(
	autoComplete: AutoCompleteTextView,
	picker: Button,
	field: KMutableProperty0<String?>,
	change: () -> Unit
) {
	setLabel(autoComplete, field.get())
	autoComplete.complete(this) { field.set(it) }
	picker.setOnClickListener { change() }
}
