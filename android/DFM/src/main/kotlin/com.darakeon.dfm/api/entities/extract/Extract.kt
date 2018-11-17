package com.darakeon.dfm.api.entities.extract

import com.google.gson.annotations.SerializedName

data class Extract(
	@SerializedName("MoveList")
	val moveList: Array<Move> = emptyArray(),

	@SerializedName("Name")
	var name: String = "",

	@SerializedName("Total")
	var total: Double = 0.0,

	@SerializedName("CanCheck")
	var canCheck: Boolean = false
) {
	override fun equals(other: Any?): Boolean {
		if (this === other) return true
		if (javaClass != other?.javaClass) return false

		other as Extract

		if (!moveList.contentEquals(other.moveList)) return false
		if (name != other.name) return false
		if (total != other.total) return false
		if (canCheck != other.canCheck) return false

		return true
	}

	override fun hashCode(): Int {
		var result = moveList.contentHashCode()
		result = 31 * result + name.hashCode()
		result = 31 * result + total.hashCode()
		result = 31 * result + canCheck.hashCode()
		return result
	}
}
