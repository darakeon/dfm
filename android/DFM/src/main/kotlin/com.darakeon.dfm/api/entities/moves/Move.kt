package com.darakeon.dfm.api.entities.moves

import com.darakeon.dfm.api.entities.Date
import com.darakeon.dfm.extensions.toDoubleByCulture
import java.util.ArrayList

class Move {
	var id: Int = 0
	var description: String? = null
	var year: Int = 0
	var month: Int = 0
	var day: Int = 0

	var nature: Int? = null
	var natureEnum
		get() = Nature.get(nature)
		set(value) { nature = value?.value }

	var warnCategory: Boolean = false
		private set

	var categoryName: String? = null

	var outUrl: String? = null
	var inUrl: String? = null

	var value: Double? = null
	var detailList: MutableList<Detail> = ArrayList()

	var checked: Boolean = false

	var isDetailed: Boolean = false

	var date: Date
		get() = Date(year, month, day)
		set(value) {
			year = value.year
			month = value.month
			day = value.day
		}

	fun add(description: String, amount: Int, value: Double) {
		val detail = Detail(description, amount, value)
		detailList.add(detail)
	}

	fun remove(description: String, amount: Int, value: Double) {
		for (detail in detailList) {
			if (detail == Detail(description, amount, value)) {
				detailList.remove(detail)
				return
			}
		}
	}

	fun setValue(value: String) {
		this.value = value.toDoubleByCulture() ?: 0.0
	}

	fun setDefaultData(accountUrl: String, useCategories: Boolean) {
		if (id == 0) {
			outUrl = accountUrl
			inUrl = accountUrl
		} else {
			warnCategory = !useCategories && categoryName != null
		}
	}

	fun clearNotUsedValues() {
		if (isDetailed) {
			value = null
		} else {
			detailList = ArrayList()
		}
	}
}
