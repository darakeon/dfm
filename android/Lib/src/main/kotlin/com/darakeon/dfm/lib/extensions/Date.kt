package com.darakeon.dfm.lib.extensions

import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Date
import java.util.Locale
import java.util.TimeZone
import kotlin.math.abs

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

fun TimeZone.toFormattedHour(): String {
	val milliseconds = abs(rawOffset)
	val seconds = milliseconds / 1000
	val minutes = seconds / 60
	val hours = minutes / 60

	val onlyMinutes = minutes - 60 * hours

	val sign =
		if (rawOffset < 0) "-"
		else if (rawOffset > 0) "+"
		else " "

	val hoursFormatted =
		hours.toString().padStart(2, '0');
	val onlyMinutesFormatted =
		onlyMinutes.toString().padStart(2, '0');

	return "UTC$sign$hoursFormatted:$onlyMinutesFormatted"
}
