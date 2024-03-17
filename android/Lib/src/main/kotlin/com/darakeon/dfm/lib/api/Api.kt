package com.darakeon.dfm.lib.api

import android.content.Context
import com.darakeon.dfm.lib.api.entities.Body
import com.darakeon.dfm.lib.api.entities.accounts.AccountList
import com.darakeon.dfm.lib.api.entities.extract.Extract
import com.darakeon.dfm.lib.api.entities.login.Login
import com.darakeon.dfm.lib.api.entities.login.Ticket
import com.darakeon.dfm.lib.api.entities.moves.Lists
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.lib.api.entities.moves.MoveCreation
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.lib.api.entities.settings.Settings
import com.darakeon.dfm.lib.api.entities.signup.SignUp
import com.darakeon.dfm.lib.api.entities.errors.ErrorList
import com.darakeon.dfm.lib.api.entities.summary.Summary
import com.darakeon.dfm.lib.api.entities.terms.Terms
import com.darakeon.dfm.lib.api.entities.tfa.TFA
import com.darakeon.dfm.lib.api.entities.wipe.Wipe
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

	private fun <T> Call<Body<T>>.callData(
		onSuccess: (T) -> Unit
	) {
		this.call(true) {
			onSuccess(it!!)
		}
	}

	private fun <T> Call<Body<T>>.callNoData(
		onSuccess: () -> Unit
	) {
		this.call(false) {
			onSuccess()
		}
	}

	private fun <T> Call<Body<T>>.call(
		hasData: Boolean,
		onSuccess: (T?) -> Unit
	) {
		val call = currentCall

		if (call != null && !call.isCanceled && !call.isExecuted) {
			return
		}

		currentCall = this
		requestHandler.call(this, hasData) {
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
		service.listAccounts().callData(onSuccess)
	}

	fun getExtract(
		accountUrl: String,
		year: Int,
		month: Int,
		onSuccess: (Extract) -> Unit
	) {
		val time = year * 100 + month + 1
		service.getExtract(accountUrl, time).callData(onSuccess)
	}

	fun check(
		id: UUID,
		nature: Nature,
		onSuccess: () -> Unit
	) {
		service.check(id, nature).callNoData(onSuccess)
	}

	fun uncheck(
		id: UUID,
		nature: Nature,
		onSuccess: () -> Unit
	) {
		service.uncheck(id, nature).callNoData(onSuccess)
	}

	fun delete(
		id: UUID,
		onSuccess: () -> Unit
	) {
		service.delete(id).callNoData(onSuccess)
	}

	fun signup(
		email: String,
		password: String,
		acceptedContract: Boolean,
		language: String,
		timeZone: String,
		onSuccess: () -> Unit
	) {
		service.signup(
			SignUp(email, password, acceptedContract, language, timeZone)
		).callNoData(onSuccess)
	}

	fun login(
		email: String,
		password: String,
		onSuccess: (Ticket) -> Unit
	) {
		service.login(Login(email, password)).callData(onSuccess)
	}

	fun logout(
		onSuccess: () -> Unit
	) {
		service.logout().callNoData(onSuccess)
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
		).callData(onSuccess)
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
		).callNoData(onSuccess)
	}

	fun getSettings(
		onSuccess: (Settings) -> Unit
	) {
		service.getSettings().callData(onSuccess)
	}

	fun saveSettings(
		settings: Settings,
		onSuccess: () -> Unit
	) {
		service.saveSettings(settings).callNoData(onSuccess)
	}

	fun getSummary(
		accountUrl: String,
		year: Int,
		onSuccess: (Summary) -> Unit
	) {
		service.getSummary(accountUrl, year).callData(onSuccess)
	}

	fun validateTFA(
		text: String,
		onSuccess: () -> Unit
	) {
		service.validateTFA(TFA(text)).callNoData(onSuccess)
	}

	fun listsForMoves(
		onSuccess: (Lists) -> Unit
	) {
		service.listsForMoves().callData(onSuccess)
	}

	fun wipe(
		wipe: Wipe,
		onSuccess: () -> Unit
	) {
		service.wipe(wipe).callNoData(onSuccess)
	}

	fun getTerms(
		onSuccess: (Terms) -> Unit
	) {
		service.getTerms().callData(onSuccess)
	}

	fun wakeUpSite(onSuccess: () -> Unit) {
		service.wakeUpSite().callNoData(onSuccess)
	}

	fun listErrors(onSuccess: (ErrorList) -> Unit) {
		service.listErrors().callData(onSuccess)
	}

	fun countErrors(onSuccess: (ErrorList) -> Unit) {
		service.countErrors().callData(onSuccess)
	}

	fun archiveErrors(id: String, onSuccess: () -> Unit) {
		service.archiveErrors(id).callNoData(onSuccess)
	}
}
