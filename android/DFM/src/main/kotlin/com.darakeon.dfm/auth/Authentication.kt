package com.darakeon.dfm.auth

import android.content.Context

private const val spKey = "Ticket"

var Context.auth: String
	get() {
		val encryptedTicket = getValue(spKey)
		return decrypt(encryptedTicket)
	}
	set(value) {
		val encryptedTicket = encrypt(value)
		setValue(spKey, encryptedTicket)
	}

fun Context.clearAuth() {
	auth = ""
}

