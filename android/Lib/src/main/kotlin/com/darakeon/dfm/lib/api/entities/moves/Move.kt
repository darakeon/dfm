package com.darakeon.dfm.lib.api.entities.moves

import com.darakeon.dfm.lib.api.entities.Date
import com.darakeon.dfm.lib.extensions.toDoubleByCulture
import java.io.Serializable
import java.util.UUID

data class Move (
	var guid: UUID? = null,
	var description: String? = null,
	var year: Int = 0,
	var month: Int = 0,
	var day: Int = 0,

	var nature: NatureEnum? = null,

	var categoryName: String? = null,

	var outUrl: String? = null,
	var inUrl: String? = null,

	var value: Double? = null,
	var conversion: Double? = null,
	var detailList: MutableList<Detail> = ArrayList(),

	var checked: Boolean = false
) : Serializable {
	var isDetailed: Boolean = false

	var natureEnum
		get() = nature?.enum
		set(value) { nature = if (value == null) null else NatureEnum(value.value) }

	var warnCategory: Boolean = false
		private set

	var date: Date
		get() = Date(year, month, day)
		set(value) {
			year = value.year
			month = value.month
			day = value.day
		}

	fun add(description: String, amount: Int, value: Double, conversion: Double?) {
		val detail = Detail(description, amount, value, conversion)
		detailList.add(detail)
	}

	fun remove(description: String, amount: Int, value: Double, conversion: Double?) {
		for (detail in detailList) {
			if (detail == Detail(description, amount, value, conversion)) {
				detailList.remove(detail)
				return
			}
		}
	}

	fun setValue(value: String) {
		this.value = value.toDoubleByCulture() ?: 0.0
	}

	fun setConversion(conversion: String) {
		this.conversion = conversion.toDoubleByCulture() ?: 0.0
	}

	fun setDefaultData(accountUrl: String, useCategories: Boolean) {
		if (guid == null) {
			if (accountUrl != "") {
				outUrl = accountUrl
			}
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
