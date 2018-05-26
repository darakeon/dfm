package com.darakeon.dfm.moves

import com.darakeon.dfm.api.entities.Move

class DescriptionWatcher(private val move: Move) : AfterTextWatcher() {

	override fun textChanged(text: String) {
		move.description = text
	}
}
