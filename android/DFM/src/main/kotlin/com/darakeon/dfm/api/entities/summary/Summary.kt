package com.darakeon.dfm.api.entities.summary

data class Summary(
	val title: String = "",
	val total: Double = 0.0,
	val monthList: Array<Month> = emptyArray()
) {
	override fun equals(other: Any?): Boolean {
		if (this === other) return true
		if (javaClass != other?.javaClass) return false

		other as Summary

		if (title != other.title) return false
		if (total != other.total) return false
		if (!monthList.contentEquals(other.monthList)) return false

		return true
	}

	override fun hashCode(): Int {
		var result = title.hashCode()
		result = 31 * result + total.hashCode()
		result = 31 * result + monthList.contentHashCode()
		return result
	}
}
