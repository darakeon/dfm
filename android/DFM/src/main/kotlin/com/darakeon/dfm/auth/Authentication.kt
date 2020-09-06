package com.darakeon.dfm.auth

import android.content.Context

class Authentication(private val context: Context) {
	private val aks = Aks(context)
	private val ticketKey = "Ticket"
	private val admKey = "Ticket"

	var ticket: String
		get() {
			val encryptedTicket = context.getValue(ticketKey)
			return aks.decrypt(encryptedTicket)
		}
		set(value) {
			val encryptedTicket = aks.encrypt(value)
			context.setValue(ticketKey, encryptedTicket)
		}

	var isAdm: Boolean
		get() {
			val encryptedTicket = context.getValue(admKey)
			return aks.decrypt(encryptedTicket) == "true"
		}
		set(value) {
			val encryptedTicket = aks.encrypt(value.toString())
			context.setValue(admKey, encryptedTicket)
		}

	fun clear() {
		ticket = ""
		isAdm = false
	}

	val isLoggedIn
		get() = ticket != ""
}
