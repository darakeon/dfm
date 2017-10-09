package com.darakeon.dfm.user

import android.content.Context

private val spKey = "Ticket"

fun Context.SetAuth(ticket: String) {
	val encryptedTicket = Encrypt(ticket)
	setValue(spKey, encryptedTicket)
}

fun Context.GetAuth(): String {
	val encryptedTicket = getValue(spKey)
	return Decrypt(encryptedTicket)
}

fun Context.IsLoggedIn(): Boolean {
	return !GetAuth().isEmpty()
}

fun Context.ClearAuth() {
	SetAuth("")
}

