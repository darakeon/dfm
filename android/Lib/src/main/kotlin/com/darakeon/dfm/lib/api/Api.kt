package com.darakeon.dfm.lib.api

import android.content.Context
import com.darakeon.dfm.lib.api.entities.Body
import com.darakeon.dfm.lib.api.entities.accounts.AccountList
import com.darakeon.dfm.lib.api.entities.extract.Extract
import com.darakeon.dfm.lib.api.entities.login.Login
import com.darakeon.dfm.lib.api.entities.moves.Lists
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.lib.api.entities.moves.MoveCreation
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.lib.api.entities.settings.Settings
import com.darakeon.dfm.lib.api.entities.status.ErrorList
import com.darakeon.dfm.lib.api.entities.summary.Summary
import retrofit2.Call
import java.util.UUID

class Api<C>(
	caller: C,
	url: String?,
)
	where C: Context, C: ApiCaller
{
	private val requestHandler = RequestHandler(caller, url)

	private val service = requestHandler.service
	private var currentCall: Call<*>? = null

	var cancelled = false
		private set

	private fun <T> Call<Body<T>>.call(onSuccess: (T) -> Unit) {
		val call = currentCall

		if (call != null && !call.isCanceled && !call.isExecuted) {
			return
		}

		currentCall = this
		requestHandler.call(this) {
			currentCall = null
			onSuccess(it)
		}
	}

	fun cancel() {
		cancelled = true
		val call = currentCall ?: return
		requestHandler.cancel(call)
		currentCall = null
	}

	fun listAccounts(
		onSuccess: (AccountList) -> Unit
	) {
		service.listAccounts().call { onSuccess(it) }
	}

	fun getExtract(
		accountUrl: String,
		year: Int,
		month: Int,
		onSuccess: (Extract) -> Unit
	) {
		val time = year * 100 + month + 1
		service.getExtract(accountUrl, time).call { onSuccess(it) }
	}

	fun check(
		id: UUID,
		nature: Nature,
		onSuccess: () -> Unit
	) {
		service.check(id, nature).call { onSuccess() }
	}

	fun uncheck(
		id: UUID,
		nature: Nature,
		onSuccess: () -> Unit
	) {
		service.uncheck(id, nature).call { onSuccess() }
	}

	fun delete(
		id: UUID,
		onSuccess: () -> Unit
	) {
		service.delete(id).call { onSuccess() }
	}

	fun login(
		email: String,
		password: String,
		onSuccess: (Login) -> Unit
	) {
		service.login(email, password).call { onSuccess(it) }
	}

	fun logout(
		onSuccess: () -> Unit
	) {
		service.logout().call { onSuccess() }
	}

	fun getMove(
		id: UUID?,
		onSuccess: (MoveCreation) -> Unit
	) {
		(
			if (id == null)
				service.getMove()
			else
				service.getMove(id)
		).call { onSuccess(it) }
	}

	fun saveMove(
		move: Move,
		onSuccess: () -> Unit
	) {
		val id = move.guid
		(
			if (id == null)
				service.saveMove(move)
			else
				service.saveMove(id, move)
		).call { onSuccess() }
	}

	fun getSettings(
		onSuccess: (Settings) -> Unit
	) {
		service.getSettings().call { onSuccess(it) }
	}

	fun saveSettings(
		settings: Settings,
		onSuccess: () -> Unit
	) {
		service.saveSettings(settings).call { onSuccess() }
	}

	fun getSummary(
		accountUrl: String,
		year: Int,
		onSuccess: (Summary) -> Unit
	) {
		service.getSummary(accountUrl, year).call { onSuccess(it) }
	}

	fun validateTFA(
		text: String,
		onSuccess: () -> Unit
	) {
		service.validateTFA(text).call { onSuccess() }
	}

	fun wakeUpSite(onSuccess: () -> Unit) {
		service.wakeUpSite().call { onSuccess() }
	}

	fun listErrors(onSuccess: (ErrorList) -> Unit) {
		service.listErrors().call { onSuccess(it) }
	}

	fun countErrors(onSuccess: (ErrorList) -> Unit) {
		service.countErrors().call { onSuccess(it) }
	}

	fun archiveErrors(id: Int, onSuccess: () -> Unit) {
		service.archiveErrors(id).call { onSuccess() }
	}

	fun listsForMoves(onSuccess: (Lists) -> Unit) {
		service.listsForMoves().call { onSuccess(it) }
	}
}
