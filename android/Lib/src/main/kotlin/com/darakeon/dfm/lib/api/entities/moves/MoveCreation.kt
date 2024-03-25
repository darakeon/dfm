package com.darakeon.dfm.lib.api.entities.moves

data class MoveCreation(
	val move: Move? = null
) {
	override fun equals(other: Any?): Boolean {
		if (this === other) return true
		if (javaClass != other?.javaClass) return false

		other as MoveCreation

		if (move != other.move) return false

		return true
	}

	override fun hashCode(): Int {
		return move.hashCode()
	}
}
