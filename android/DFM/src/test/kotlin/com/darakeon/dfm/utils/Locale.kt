package com.darakeon.dfm.utils

import java.text.DecimalFormat
import java.util.Locale

fun String.getDecimal(): String {
	val separator = Locale.getDefault()
	val formatter = DecimalFormat.getInstance(separator) as DecimalFormat
	val decimal = formatter.decimalFormatSymbols.decimalSeparator.toString()

	return replace(".", decimal)
}
