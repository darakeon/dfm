package com.darakeon.dfm.moves

import android.content.Context
import android.content.Intent
import android.os.SystemClock
import androidx.core.app.JobIntentService
import com.darakeon.dfm.R
import com.darakeon.dfm.lib.Log
import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.api.ApiCaller
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.lib.auth.Authentication
import java.io.Serializable

class MovesService : JobIntentService(), ApiCaller {
	private val intervalMin
		get() = resources.getInteger(
			R.integer.service_seconds_interval
		) / 60.0

	private val intervalMs
		get() = (intervalMin * 60 * 1000).toLong()

	private val moveOffline
		get() = MovesOffline(this)
	private lateinit var move: Move

	override fun onHandleWork(intent: Intent) {
		val noMove = intent.getSerializableExtra("no_move") as Boolean?

		if (noMove == true) {
			next()
			return
		}

		val move = intent.getSerializableExtra("move") as Move?

		if (move == null) {
			Log.record("No move received")
			next()
			return
		}

		this.move = move

		moveOffline.add(move)

		Api(this, null).saveMove(move) {
			moveOffline.success(move)
			next()
		}
	}

	private val auth = Authentication(this)

	override var ticket: String
		get() = auth.ticket
		set(value) { auth.ticket = value }

	override fun logout() {
		auth.clear()
	}

	override fun checkTFA() {
		moveOffline.error(move)
	}

	override fun error(text: String) {
		moveOffline.error(move, text)
		next()
	}

	override fun error(resId: Int) {
		Log.record("Error thrown: ${getString(resId)}")
		restart(move)
	}

	override fun error(resMessage: Int, resButton: Int, action: () -> Unit) {
		moveOffline.error(move, resMessage)
		action()
		next()
	}

	override fun error(url: String, error: Throwable) {
		Log.record("Error thrown: $url $error")
		restart(move)
	}

	private fun next() {
		val otherMove = moveOffline.next
		if (otherMove != null) {
			restart(otherMove.move)
		} else {
			Log.record("end of jobs")
		}
		stopSelf()
	}

	private fun restart(move: Move) {
		if (move.hashCode() == this.move.hashCode()) {
			Log.record("...${intervalMin}min...")
			SystemClock.sleep(intervalMs)
		}

		if (isStopped) return

		start(this, move)
	}

	companion object {
		fun start(context: Context, move: Move) {
			start(context, "move", move)
		}

		fun start(context: Context) {
			val offline = MovesOffline(context)
			if (offline.run) {
				start(context, "no_move", true)
			} else {
				offline.clearSucceeded()
			}
		}

		private fun start(context: Context, key: String, value: Serializable) {
			val intent = Intent()
			intent.putExtra(key, value)
			enqueueWork(context, MovesService::class.java, 27, intent)
		}
	}
}
