package com.darakeon.dfm.moves

import com.darakeon.dfm.lib.api.entities.moves.Move

data class MoveWithStatus(
        val move: Move,
        var status: MoveStatus = MoveStatus.Pending,
        var error: String = ""
) {
	val pending
		get() = status == MoveStatus.Pending

	fun success() {
		status = MoveStatus.Success
	}

	fun error(error: String) {
		status = MoveStatus.Error
		this.error = error
	}

	fun has(move: Move): Boolean =
		this.move.hashCode() == move.hashCode()
}
