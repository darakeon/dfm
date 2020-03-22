package com.darakeon.dfm.api.entities.accounts

data class AccountList(
	val accountList: Array<Account>
) {
	override fun equals(other: Any?): Boolean {
		if (this === other) return true
		if (javaClass != other?.javaClass) return false

		other as AccountList

		if (!accountList.contentEquals(other.accountList)) return false

		return true
	}

	override fun hashCode(): Int {
		return accountList.contentHashCode()
	}
}