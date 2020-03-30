package com.darakeon.dfm.api.entities.moves

import com.darakeon.dfm.api.entities.ComboItem

data class MoveCreation(
	override val isUsingCategories: Boolean = false,

	override val categoryList: Array<ComboItem> = emptyArray(),
	override val natureList: Array<ComboItem> = emptyArray(),
	override val accountList: Array<ComboItem> = emptyArray(),

	val move: Move? = null
): MoveForm {
	override fun blockedByAccounts() =
		accountList.isEmpty()

	override fun blockedByCategories() =
		isUsingCategories && categoryList.isEmpty()

	override fun equals(other: Any?): Boolean {
		if (this === other) return true
		if (javaClass != other?.javaClass) return false

		other as MoveCreation

		if (move != other.move) return false
		if (isUsingCategories != other.isUsingCategories) return false

		if (!categoryList.contentEquals(other.categoryList)) return false
		if (!natureList.contentEquals(other.natureList)) return false
		if (!accountList.contentEquals(other.accountList)) return false

		return true
	}

	override fun hashCode(): Int {
		var result = move.hashCode()
		result = 31 * result + isUsingCategories.hashCode()
		result = 31 * result + categoryList.contentHashCode()
		result = 31 * result + natureList.contentHashCode()
		result = 31 * result + accountList.contentHashCode()
		return result
	}
}
