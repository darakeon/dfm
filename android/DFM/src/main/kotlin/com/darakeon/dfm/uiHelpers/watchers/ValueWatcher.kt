package com.darakeon.dfm.uiHelpers.watchers

import com.darakeon.dfm.api.entities.Move
import com.darakeon.dfm.api.toDoubleByCulture

class ValueWatcher(private val move: Move) : AfterTextWatcher() {

	override fun textChanged(text: String) {
		move.value = text.toDoubleByCulture() ?: 0.toDouble()
	}
}
