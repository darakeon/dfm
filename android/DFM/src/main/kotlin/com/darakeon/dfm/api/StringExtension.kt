package com.darakeon.dfm.api

import java.text.NumberFormat
import java.text.ParseException
import java.util.*

fun String.toDoubleByCulture() : Double?
{
	try {
		val format = NumberFormat.getInstance(Locale.getDefault());
		return format.parse(this).toDouble()
	}
	catch (e: NumberFormatException)
	{
		return tryGetByDefault()
	}
	catch (e: ParseException)
	{
		return tryGetByDefault()
	}
}

private fun String.tryGetByDefault(): Double? {
	try {
		return this.toDouble()
	} catch (e: NumberFormatException) {
		return null
	}
}