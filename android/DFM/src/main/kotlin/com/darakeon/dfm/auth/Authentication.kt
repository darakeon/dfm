package com.darakeon.dfm.auth

import android.content.Context

class Authentication(private val context: Context) {
	private val aks = Aks(context)
	private val spKey = "Ticket"

	var ticket: String
		get() {
			val encryptedTicket = context.getValue(spKey)
			return aks.decrypt(encryptedTicket)
		}
		set(value) {
			val encryptedTicket = aks.encrypt(value)
			context.setValue(spKey, encryptedTicket)
		}

	fun clear() {
		ticket = ""
	}

	val isLoggedIn
		get() = ticket != ""
}
