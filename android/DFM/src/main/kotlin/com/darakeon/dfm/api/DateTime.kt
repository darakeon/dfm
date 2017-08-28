package com.darakeon.dfm.api

import org.json.JSONException
import org.json.JSONObject
import java.util.*

object DateTime {
    @Throws(JSONException::class)
    fun getCalendar(date: JSONObject): Calendar {
        val calendar = Calendar.getInstance()
        calendar.set(Calendar.YEAR, date.getInt("Year"))
        calendar.set(Calendar.MONTH, date.getInt("Month") - 1)
        calendar.set(Calendar.DAY_OF_MONTH, date.getInt("Day"))

        return calendar
    }
}
