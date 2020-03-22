package com.darakeon.dfm.api.entities.extract

import com.darakeon.dfm.api.entities.Date

data class Move(
	val description: String,
	val year: Int,
	val month: Int,
	val day: Int,
	val total: Double,
	var checked: Boolean,
	val id: Int
) {
	val date: Date
		get() = Date(year, month, day)
}
