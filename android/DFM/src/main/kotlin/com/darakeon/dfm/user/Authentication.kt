package com.darakeon.dfm.user

import android.content.Context

class Authentication(private val context: Context) {
	private val safe: SafeTicket = SafeTicket(this.context)


	fun Set(ticket: String) {
		val encryptedTicket = safe.Encrypt(ticket)
		SP.setValue(context, spKey, encryptedTicket)
	}

	fun Get(): String {
		val encryptedTicket = SP.getValue(context, spKey)
		return safe.Decrypt(encryptedTicket)
	}

	fun IsLoggedIn(): Boolean {
		return !Get().isEmpty()
	}

	fun Clear() {
		Set("")
	}

	companion object {

		private val spKey = "Ticket"
	}


}