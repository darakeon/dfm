package com.darakeon.dfm.accounts

import android.os.Bundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.Account
import com.darakeon.dfm.auth.getAuth
import com.darakeon.dfm.base.BaseActivity
import kotlinx.android.synthetic.main.accounts.empty_list
import kotlinx.android.synthetic.main.accounts.main_table

class AccountsActivity : BaseActivity<AccountsStatic>(AccountsStatic) {
	override val contentView = R.layout.accounts

	private var accountList: List<Account>? = null

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (rotated && static.succeeded) {
			fillAccounts()
		} else {
			api.listAccounts(
				getAuth(),
				this::handleAccounts,
				{ throw it }
			)
		}
	}

	private fun handleAccounts(accountList: List<Account>) {
		static.accountList = accountList
		fillAccounts()
	}

	private fun fillAccounts() {
		if (static.accountList.isEmpty()) {
			main_table.visibility = View.GONE
			empty_list.visibility = View.VISIBLE
		} else {
			main_table.visibility = View.VISIBLE
			empty_list.visibility = View.GONE

			main_table.adapter = AccountAdapter(this, static.accountList)
		}
	}


}
