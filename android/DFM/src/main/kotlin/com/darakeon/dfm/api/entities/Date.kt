package com.darakeon.dfm.api.entities

import com.darakeon.dfm.extensions.format
import java.util.Calendar

data class Date(
	val year: Int,
	val month: Int,
	val day: Int
) {
	constructor() : this(Calendar.getInstance())

	constructor(calendar: Calendar) : this(
		calendar.get(Calendar.YEAR),
		calendar.get(Calendar.MONTH) + 1,
		calendar.get(Calendar.DAY_OF_MONTH)
	)

	val javaMonth get() = month - 1

	fun format(): String {
		val calendar = Calendar.getInstance()

		calendar.set(Calendar.YEAR, year)
		calendar.set(Calendar.MONTH, javaMonth)
		calendar.set(Calendar.DAY_OF_MONTH, day)

		return calendar.format()
	}
}
