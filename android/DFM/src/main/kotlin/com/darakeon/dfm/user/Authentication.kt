package com.darakeon.dfm.user

import android.content.Context

private val spKey = "Ticket"

fun Context.setAuth(ticket: String) {
	val encryptedTicket = encrypt(ticket)
	setValue(spKey, encryptedTicket)
}

fun Context.getAuth(): String {
	val encryptedTicket = getValue(spKey)
	return decrypt(encryptedTicket)
}

fun Context.isLoggedIn(): Boolean = !getAuth().isEmpty()

fun Context.clearAuth() {
	setAuth("")
}

