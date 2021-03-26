package com.darakeon.dfm.lib.extensions

import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Date
import java.util.Locale

fun Calendar.format(): String {
	return time.format()
}

fun Date.format(): String {
	val formatter = SimpleDateFormat("yyyy-MM-dd", Locale.getDefault())
	return formatter.format(time)
}

fun Calendar.formatNoDay(): String {
	return time.formatNoDay()
}

fun Date.formatNoDay(): String {
	val formatter = SimpleDateFormat("MMM/yyyy", Locale.getDefault())
	return formatter.format(time).replace(".", "")
}
