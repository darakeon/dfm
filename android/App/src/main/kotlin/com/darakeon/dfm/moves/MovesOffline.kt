package com.darakeon.dfm.moves

import android.content.Context
import com.darakeon.dfm.R
import com.darakeon.dfm.lib.Log
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.lib.auth.getValue
import com.darakeon.dfm.lib.auth.setValue
import com.google.gson.Gson

class MovesOffline(private val context: Context) {
	private var spMoves: String
		get() = context.getValue("moves")
		set(value) = context.setValue("moves", value)

	private var stati: Array<MoveWithStatus>
		get() = if (spMoves.isEmpty()) {
			emptyArray()
		} else {
			Gson().fromJson(
				spMoves,
				Array<MoveWithStatus>::class.java
			)
		}
		set(value) {
			spMoves = Gson().toJson(value)
		}

	val next: MoveWithStatus?
		get() {
			val pending = stati.filter { m -> m.pending }
			Log.record("Pending: ${pending.size}")
			return pending.firstOrNull()
		}

	val run: Boolean
		get() = stati.any { m -> m.pending }

	val error: MoveWithStatus?
		get() = stati.firstOrNull {
			m -> m.status == MoveStatus.Error
		}

	fun add(move: Move) {
		if (stati.any { m -> m.has(move) && m.pending}) {
			Log.record("Already added")
		} else {
			stati += MoveWithStatus(move)
		}
	}

	fun success(move: Move) {
		Log.record("[SUCCEEDED]: $move")
		update(move) {
			it.success()
		}
	}

	fun error(move: Move) =
		error(move, R.string.fail_at_offline_insert)

	fun error(move: Move, error: Int) =
		error(move, context.getString(error))

	fun error(move: Move, error: String) {
		Log.record("[$error]: $move")
		update(move) {
			it.error(error)
		}
	}

	private fun update(move: Move, action: (MoveWithStatus) -> Unit) {
		val list = stati
		val item = list.first { m -> m.has(move) && m.pending }
		action(item)
		stati = list
	}

	fun remove(moveWithStatusHash: Int) {
		stati = stati.filter {
			it.hashCode() != moveWithStatusHash
		}.toTypedArray()

		val pending = stati.filter { it.pending }.size
		Log.record("Pending: $pending")
	}

	fun clearSucceeded() {
		stati = stati.filter {
			it.status != MoveStatus.Success
		}.toTypedArray()
		Log.record("Stack: ${stati.size}")
	}
}
