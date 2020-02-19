package com.darakeon.dfm.auth

import android.content.Context
import android.os.Build

class Authentication(private val context: Context) {
	private val aks =
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M)
			Aks()
		else
			OldAks(context)

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
}
