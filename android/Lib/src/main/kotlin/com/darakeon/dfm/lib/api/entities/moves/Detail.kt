package com.darakeon.dfm.lib.api.entities.moves

class Detail(
	val description: String = "",
	val amount: Int = 0,
	val value: Double = 0.0
) {
	override fun equals(other: Any?): Boolean {
		return other is Detail
			&& description == other.description
			&& amount == other.amount
			&& value == other.value
	}

	override fun hashCode(): Int {
		var result = description.hashCode()
		result = 31 * result + amount
		result = 31 * result + value.hashCode()
		return result
	}
}
