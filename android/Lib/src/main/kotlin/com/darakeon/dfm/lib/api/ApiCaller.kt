package com.darakeon.dfm.lib.api

interface ApiCaller {
	interface Api

	val ticket: String
	val api: Api?

	fun logout()
	fun checkTFA()

	fun startWait() {}
	fun endWait() {}

	fun error(text: String)
	fun error(resId: Int)
	fun error(resId: Int, action: () -> Unit)
	fun error(url: String, error: Throwable)
}
