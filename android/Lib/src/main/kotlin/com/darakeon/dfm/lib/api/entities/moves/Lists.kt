package com.darakeon.dfm.lib.api.entities.moves

import com.darakeon.dfm.lib.api.entities.AccountComboItem
import com.darakeon.dfm.lib.api.entities.ComboItem

data class Lists(
	val isUsingCategories: Boolean,
	val accountList: Array<AccountComboItem>,
	val categoryList: Array<ComboItem>,
) {
	override fun equals(other: Any?): Boolean {
		if (this === other) return true
		if (javaClass != other?.javaClass) return false

		other as Lists

		if (isUsingCategories != other.isUsingCategories) return false
		if (!accountList.contentEquals(other.accountList)) return false
		if (!categoryList.contentEquals(other.categoryList)) return false

		return true
	}

	override fun hashCode(): Int {
		var result = isUsingCategories.hashCode()
		result = 31 * result + accountList.contentHashCode()
		result = 31 * result + categoryList.contentHashCode()
		return result
	}
}
