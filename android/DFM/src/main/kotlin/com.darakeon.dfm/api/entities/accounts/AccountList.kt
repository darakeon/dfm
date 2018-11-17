package com.darakeon.dfm.api.entities.accounts

import com.google.gson.annotations.SerializedName

data class AccountList(
	@SerializedName("AccountList")
	val list: Array<Account>
) {
	override fun equals(other: Any?): Boolean {
		if (this === other) return true
		if (javaClass != other?.javaClass) return false

		other as AccountList

		if (!list.contentEquals(other.list)) return false

		return true
	}

	override fun hashCode(): Int {
		return list.contentHashCode()
	}
}