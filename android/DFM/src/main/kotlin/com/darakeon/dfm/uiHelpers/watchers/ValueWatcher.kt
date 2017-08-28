package com.darakeon.dfm.uiHelpers.watchers

import com.darakeon.dfm.api.entities.Move

class ValueWatcher(private val move: Move) : AfterTextWatcher() {

	override fun textChanged(text: String) {
		try {
			move.Value = java.lang.Double.parseDouble(text)
		} catch (ignored: NumberFormatException) {
		}

	}
}
