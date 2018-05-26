package com.darakeon.dfm.moves

import com.darakeon.dfm.api.entities.Move
import com.darakeon.dfm.api.toDoubleByCulture
import com.darakeon.dfm.moves.AfterTextWatcher

class ValueWatcher(private val move: Move) : AfterTextWatcher() {

	override fun textChanged(text: String) {
		move.value = text.toDoubleByCulture() ?: 0.toDouble()
	}
}
