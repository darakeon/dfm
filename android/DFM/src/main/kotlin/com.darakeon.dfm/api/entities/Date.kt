package com.darakeon.dfm.api.entities

import com.darakeon.dfm.extensions.format
import com.google.gson.annotations.SerializedName
import java.util.Calendar

data class Date(
	@SerializedName("Year")
	val year: Int,

	@SerializedName("Month")
	val month: Int,

	@SerializedName("Day")
	val day: Int
) {
	constructor() : this(
		Calendar.getInstance().get(Calendar.YEAR),
		Calendar.getInstance().get(Calendar.MONTH),
		Calendar.getInstance().get(Calendar.DAY_OF_MONTH)
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
