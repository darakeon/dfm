package com.darakeon.dfm.auth

import android.content.Context

private val spKey = "Ticket"

fun Context.setAuth(ticket: String) {
	val encryptedTicket = encrypt(ticket)
	setValue(spKey, encryptedTicket)
}

val Context.auth: String get() {
	val encryptedTicket = getValue(spKey)
	return decrypt(encryptedTicket)
}

fun Context.clearAuth() {
	setAuth("")
}

