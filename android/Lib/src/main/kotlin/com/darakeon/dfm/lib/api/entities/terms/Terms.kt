package com.darakeon.dfm.lib.api.entities.terms

import com.darakeon.dfm.lib.api.entities.Date

data class Terms (
	var year: Int = 0,
	var month: Int = 0,
	var day: Int = 0,
	var content: Clause = Clause(),
) {
	var date: Date
		get() = Date(year, month, day)
		set(value) {
			year = value.year
			month = value.month
			day = value.day
		}
}
