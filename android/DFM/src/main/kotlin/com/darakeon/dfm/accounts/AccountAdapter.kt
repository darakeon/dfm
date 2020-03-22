package com.darakeon.dfm.accounts

import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.accounts.Account
import com.darakeon.dfm.base.Adapter

class AccountAdapter(
	activity: AccountsActivity,
	private val accountList: Array<Account>
) : Adapter<AccountsActivity, Account, AccountLine>(activity, accountList) {
	override val id: Int
		get() = R.layout.accounts_line

	override fun populateView(view: AccountLine, position: Int) =
		view.setAccount(accountList[position])
}
