package com.darakeon.dfm.lib.api.entities.status

import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Locale

fun Throwable.toErrorLog(): ErrorLog {
	val date = Calendar.getInstance()

	val id = SimpleDateFormat(
		"yyyyMMddHHmmssffffff",
		Locale.getDefault()
	).format(date)

	return ErrorLog(id, this.toExcept())
}

fun Throwable.toExcept() =
	Except(
		this::class.qualifiedName ?: "",
		message ?: "No message",
		exceptCause(),
		stackTraceToString(),
		""
	)

fun Throwable.exceptCause(): Except? {
	return if (cause == this)
		null
	else
		cause?.toExcept()
}
