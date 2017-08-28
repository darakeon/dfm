package com.dontflymoney.userdata

import android.content.Context

import java.util.Locale

internal class SafeTicket(context: Context) {
    private var key: String? = null

    init {
        var machineId = Unique.GetKey(context)

        if (machineId.length > 13) {
            val remove = machineId.length - 13
            machineId = machineId.substring(remove)
        }

        val factor = java.lang.Long.parseLong(machineId, 16) * 27
        key = java.lang.Long.toHexString(factor)
        key = key!!.toUpperCase(Locale.ITALY)

        key += key
        key += key
    }


    fun Encrypt(ticket: String?): String? {
        if (ticket == null)
            return null

        var encryptedTicket = ""

        for (s in 0..ticket.length - 1) {
            encryptedTicket += key!!.substring(s, s + 1) + ticket.substring(s, s + 1)
        }

        return encryptedTicket
    }

    fun Decrypt(encryptedTicket: String?): String? {
        if (encryptedTicket == null)
            return null

        var ticket: String? = ""

        var s = 0
        while (s < encryptedTicket.length) {
            val keyChar = key!!.substring(s / 2, s / 2 + 1)
            val encryptedChar = encryptedTicket.substring(s, s + 1)

            if (keyChar != encryptedChar) {
                ticket = null
                break
            }

            ticket += encryptedTicket.substring(s + 1, s + 2)
            s += 2
        }

        return ticket

    }


}
