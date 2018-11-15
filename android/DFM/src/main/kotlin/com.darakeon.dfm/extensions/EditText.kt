package com.darakeon.dfm.extensions

import android.widget.EditText
import com.darakeon.dfm.moves.AfterTextWatcher

fun EditText.onChange(
	onTextChanged: (String) -> Unit
) {
	addTextChangedListener(object : AfterTextWatcher() {
		override fun textChanged(text: String) {
			onTextChanged(text)
		}
	})
}