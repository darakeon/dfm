package com.darakeon.dfm.auth

import android.content.Context
import com.google.android.gms.iid.InstanceID

private fun Context.getKey(): String {
	var result = InstanceID.getInstance(this).id

	result += result
	result += result

	return result
}

fun Context.encrypt(ticket: String): String {
	var encryptedTicket = ""
	val key = getKey()

	for (s in 0 until ticket.length) {
		encryptedTicket += key.substring(s, s + 1) + ticket.substring(s, s + 1)
	}

	return encryptedTicket
}

fun Context.decrypt(encryptedTicket: String): String {
	var ticket = ""
	val key = getKey()

	var s = 0
	while (s < encryptedTicket.length) {
		val keyChar = key.substring(s / 2, s / 2 + 1)
		val encryptedChar = encryptedTicket.substring(s, s + 1)

		if (keyChar != encryptedChar) {
			ticket = ""
			break
		}

		ticket += encryptedTicket.substring(s + 1, s + 2)
		s += 2
	}

	return ticket

}