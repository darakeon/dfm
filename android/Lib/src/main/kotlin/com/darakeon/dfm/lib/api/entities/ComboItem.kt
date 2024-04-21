package com.darakeon.dfm.lib.api.entities

import android.widget.AutoCompleteTextView
import android.widget.Button
import android.widget.TextView
import com.darakeon.dfm.lib.extensions.complete
import kotlin.reflect.KMutableProperty0

open class ComboItem(
	val text: String,
	val value: String?
)

class AccountComboItem(
	val currency: String,
	text: String,
	value: String?,
) : ComboItem(text, value)

fun <T: ComboItem> Array<T>.setLabel(field: TextView, value: String?) {
	val saved = this.firstOrNull {
		it.value == value
	} ?: return

	field.text = saved.text
}

fun <T: ComboItem> Array<T>.setCombo(
	autoComplete: AutoCompleteTextView,
	picker: Button,
	field: KMutableProperty0<String?>,
	change: () -> Unit
) {
	setLabel(autoComplete, field.get())
	autoComplete.complete(this) { field.set(it) }
	picker.setOnClickListener { change() }
}
