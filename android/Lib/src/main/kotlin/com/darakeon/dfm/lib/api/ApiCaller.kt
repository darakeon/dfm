package com.darakeon.dfm.lib.api

interface ApiCaller {
	val ticket: String

	fun logout()
	fun checkTFA()

	fun startWait() {}
	fun endWait() {}

	fun error(text: String)
	fun error(resId: Int)
	fun error(resId: Int, action: () -> Unit)
	fun error(url: String, error: Throwable)
}
