package com.darakeon.dfm.lib.api

import com.darakeon.dfm.lib.R

interface ApiCaller {
	val ticket: String

	fun logout()
	fun checkTFA()

	fun startWait() {}
	fun endWait() {}

	fun offline() = error(R.string.connection_fail)
	fun error(text: String)
	fun error(resId: Int)
	fun error(resMessage: Int, resButton: Int, action: () -> Unit)
	fun error(url: String, error: Throwable)
}
