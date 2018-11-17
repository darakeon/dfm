package com.darakeon.dfm.api.entities.moves

import com.darakeon.dfm.api.entities.ComboItem
import com.google.gson.annotations.SerializedName

data class MoveCreation(
	@SerializedName("Move")
	val move: Move? = Move(),

	@SerializedName("IsUsingCategories")
	val useCategories: Boolean = false,

	@SerializedName("CategoryList")
	val categoryList: Array<ComboItem>? = emptyArray(),

	@SerializedName("NatureList")
	val natureList: Array<ComboItem> = emptyArray(),

	@SerializedName("AccountList")
	val accountList: Array<ComboItem> = emptyArray()
) {
	override fun equals(other: Any?): Boolean {
		if (this === other) return true
		if (javaClass != other?.javaClass) return false

		other as MoveCreation

		if (move != other.move) return false
		if (useCategories != other.useCategories) return false

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
		result = 31 * result + useCategories.hashCode()
		result = 31 * result + (categoryList?.contentHashCode() ?: 0)
		result = 31 * result + natureList.contentHashCode()
		result = 31 * result + accountList.contentHashCode()
		return result
	}
}
