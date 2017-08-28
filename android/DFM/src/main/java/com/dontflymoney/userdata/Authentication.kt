package com.dontflymoney.userdata

import android.content.Context

class Authentication(private val context: Context) {
    private val safe: SafeTicket


    init {
        safe = SafeTicket(this.context)
    }

    fun Set(ticket: String?) {
        val encryptedTicket = safe.Encrypt(ticket)
        SP.setValue(context, spKey, encryptedTicket)
    }

    fun Get(): String? {
        val encryptedTicket = SP.getValue(context, spKey)
        return safe.Decrypt(encryptedTicket)
    }

    fun IsLoggedIn(): Boolean {
        val ticket = Get()

        return ticket != null && !ticket.isEmpty()
    }

    fun Clear() {
        Set(null)
    }

    companion object {

        private val spKey = "Ticket"
    }


}