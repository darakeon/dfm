package com.darakeon.dfm.uiHelpers.watchers

import android.text.Editable
import android.text.TextWatcher

abstract class AfterTextWatcher : TextWatcher {

	abstract fun textChanged(text: String)

	override fun afterTextChanged(s: Editable) {
		textChanged(s.toString())
	}

	override fun beforeTextChanged(s: CharSequence, start: Int, count: Int, after: Int) {}
	override fun onTextChanged(s: CharSequence, start: Int, before: Int, count: Int) {}
}
