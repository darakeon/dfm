package com.darakeon.dfm.lib.api.entities.extract

import com.darakeon.dfm.lib.api.entities.Date
import java.util.UUID

data class Move(
	val description: String,
	val year: Int,
	val month: Int,
	val day: Int,
	val total: Double,
	var checked: Boolean,
	val guid: UUID
) {
	val date: Date
		get() = Date(year, month, day)
}
