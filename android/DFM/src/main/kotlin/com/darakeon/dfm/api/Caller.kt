package com.darakeon.dfm.api

interface Caller {
	val ticket: String
	fun logout()
	fun error(text: String)
	fun error(resId: Int)
	fun error(resId: Int, action: () -> Unit)
	fun error(url: String, error: Throwable)
}
