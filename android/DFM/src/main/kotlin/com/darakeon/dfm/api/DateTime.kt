package com.darakeon.dfm.api

import org.json.JSONObject
import java.util.*

fun JSONObject.getCalendar(name: String): Calendar {
	val child = getJSONObject(name)

	val calendar = Calendar.getInstance()
	calendar.set(Calendar.YEAR, child.getInt("Year"))
	calendar.set(Calendar.MONTH, child.getInt("Month") - 1)
	calendar.set(Calendar.DAY_OF_MONTH, child.getInt("Day"))

	return calendar
}
