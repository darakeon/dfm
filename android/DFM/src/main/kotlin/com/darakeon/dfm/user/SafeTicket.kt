package com.darakeon.dfm.user

import android.content.Context
import com.google.android.gms.iid.InstanceID
import java.util.*

private fun Context.getKey(): String {
	var result = InstanceID.getInstance(this).id

	result += result
	result += result

	return result
}

fun Context.Encrypt(ticket: String): String {
	var encryptedTicket = ""
	val key = getKey()

	for (s in 0..ticket.length - 1) {
		encryptedTicket += key.substring(s, s + 1) + ticket.substring(s, s + 1)
	}

	return encryptedTicket
}

fun Context.Decrypt(encryptedTicket: String): String {
	var ticket: String = ""
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