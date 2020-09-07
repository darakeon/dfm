package com.darakeon.dfm.lib.extensions

import java.text.NumberFormat
import java.util.Locale

fun String.toDoubleByCulture() : Double?
{
	return try {
		val format = NumberFormat.getInstance(Locale.getDefault())
		format.parse(this)?.toDouble()
	} catch (e: Exception) {
		tryGetByDefault()
	}
}

private fun String.tryGetByDefault(): Double? {
	return try {
		this.toDouble()
	} catch (e: NumberFormatException) {
		null
	}
}
