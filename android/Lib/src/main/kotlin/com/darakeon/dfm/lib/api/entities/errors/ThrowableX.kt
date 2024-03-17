package com.darakeon.dfm.lib.api.entities.errors

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
