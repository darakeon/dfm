package com.darakeon.dfm.api.entities.moves

import com.darakeon.dfm.api.entities.ComboItem

data class MoveCreation(
	val move: Move? = Move(),

	val isUsingCategories: Boolean = false,

	val categoryList: Array<ComboItem>? = emptyArray(),
	val natureList: Array<ComboItem> = emptyArray(),
	val accountList: Array<ComboItem> = emptyArray()
) {
	override fun equals(other: Any?): Boolean {
		if (this === other) return true
		if (javaClass != other?.javaClass) return false

		other as MoveCreation

		if (move != other.move) return false
		if (isUsingCategories != other.isUsingCategories) return false

		val catList = categoryList
		val otherCatList = other.categoryList
		if (catList != null) {
			if (otherCatList == null) return false
			if (!catList.contentEquals(otherCatList)) return false
		} else if (otherCatList != null) return false

		if (!natureList.contentEquals(other.natureList)) return false
		if (!accountList.contentEquals(other.accountList)) return false

		return true
	}

	override fun hashCode(): Int {
		var result = move.hashCode()
		result = 31 * result + isUsingCategories.hashCode()
		result = 31 * result + (categoryList?.contentHashCode() ?: 0)
		result = 31 * result + natureList.contentHashCode()
		result = 31 * result + accountList.contentHashCode()
		return result
	}
}
