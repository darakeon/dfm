package com.darakeon.dfm.moves

import com.darakeon.dfm.api.old.entities.Move
import com.darakeon.dfm.extensions.toDoubleByCulture

class ValueWatcher(private val move: Move) : AfterTextWatcher() {

	override fun textChanged(text: String) {
		move.value = text.toDoubleByCulture() ?: 0.toDouble()
	}
}
