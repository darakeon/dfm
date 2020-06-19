package com.darakeon.dfm.api.entities

import com.darakeon.dfm.extensions.format
import java.lang.Integer.parseInt
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

	constructor(date: String) : this(
		split(date, 0),
		split(date, 1),
		split(date, 2)
	)

	val javaMonth get() = month - 1

	fun format(): String {
		if (year + month + day == 0)
			return ""

		val calendar = Calendar.getInstance()

		calendar.set(Calendar.YEAR, year)
		calendar.set(Calendar.MONTH, javaMonth)
		calendar.set(Calendar.DAY_OF_MONTH, day)

		return calendar.format()
	}

	companion object {
		private fun split(date: String, position: Int): Int {
			val numbers = date.split('-')
			return if (numbers.count() > position && numbers[position] != "")
				parseInt(numbers[position])
			else
				0
		}
	}
}
