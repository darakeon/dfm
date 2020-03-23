package com.darakeon.dfm.api

import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.api.entities.accounts.AccountList
import com.darakeon.dfm.api.entities.extract.Extract
import com.darakeon.dfm.api.entities.login.Login
import com.darakeon.dfm.api.entities.moves.Move
import com.darakeon.dfm.api.entities.moves.MoveCreation
import com.darakeon.dfm.api.entities.moves.Nature
import com.darakeon.dfm.api.entities.settings.Settings
import com.darakeon.dfm.api.entities.summary.Summary
import com.darakeon.dfm.base.BaseActivity
import retrofit2.Call

class Api(activity: BaseActivity) {
	private val requestHandler = RequestHandler(activity)
	private val service = requestHandler.service
	private var currentCall: Call<*>? = null

	var cancelled = false
		private set

	private fun <T> Call<Body<T>>.call(onSuccess: (T) -> Unit) {
		currentCall = this
		requestHandler.call(this, onSuccess)
	}

	fun cancel() {
		cancelled = true
		requestHandler.cancel(currentCall)
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
		id: Int,
		nature: Nature,
		onSuccess: () -> Unit
	) {
		service.check(id, nature).call { onSuccess() }
	}

	fun uncheck(
		id: Int,
		nature: Nature,
		onSuccess: () -> Unit
	) {
		service.uncheck(id, nature).call { onSuccess() }
	}

	fun delete(
		id: Int,
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
		id: Int,
		onSuccess: (MoveCreation) -> Unit
	) {
		service.getMove(id).call { onSuccess(it) }
	}

	fun saveMove(
		move: Move,
		onSuccess: () -> Unit
	) {
		service.saveMove(move.id, move).call { onSuccess() }
	}

	fun getConfig(
		onSuccess: (Settings) -> Unit
	) {
		service.getConfig().call { onSuccess(it) }
	}

	fun saveConfig(
		settings: Settings,
		onSuccess: () -> Unit
	) {
		service.saveConfig(settings).call { onSuccess() }
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
}
