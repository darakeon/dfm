package com.darakeon.dfm.accounts

import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.accounts.Account
import com.darakeon.dfm.base.Adapter

class AccountAdapter(
	activity: AccountsActivity,
	list: List<Account>
) : Adapter<AccountsActivity, Account, AccountLine>(activity, list) {
	override val id: Int
		get() = R.layout.accounts_line

	override fun populateView(view: AccountLine, position: Int) =
		view.setAccount(list[position])
}
