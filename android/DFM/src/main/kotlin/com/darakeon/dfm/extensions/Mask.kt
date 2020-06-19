package com.darakeon.dfm.extensions

import android.text.Editable
import android.text.TextWatcher
import android.widget.EditText

private class Mask(private val mask: String) : TextWatcher {
	private var isRunning = false
	private var isDeleting = false

	override fun beforeTextChanged(charSequence: CharSequence, start: Int, count: Int, after: Int) {
		isDeleting = count > after
	}

	override fun onTextChanged(charSequence: CharSequence, start: Int, before: Int, count: Int) {}
	override fun afterTextChanged(editable: Editable) {
		val length = editable.length

		if (isRunning || isDeleting || length == 0)
			return

		isRunning = true

		if (length < mask.length) {
			if (mask[length] != '#') {
				editable.append(mask[length])
			} else if (mask[length - 1] != '#') {
				editable.insert(length - 1, mask, length - 1, length)
			}
		} else {
			editable.delete(mask.length, length)
		}

		isRunning = false
	}
}

fun EditText.addMask(pattern: String) {
	addTextChangedListener(Mask(pattern))
}
