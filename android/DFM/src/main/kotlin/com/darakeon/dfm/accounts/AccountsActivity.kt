package com.darakeon.dfm.accounts

import android.os.Bundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.accounts.Account
import com.darakeon.dfm.api.entities.accounts.AccountList
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import kotlinx.android.synthetic.main.accounts.empty_list
import kotlinx.android.synthetic.main.accounts.main_table

class AccountsActivity : BaseActivity() {
	override val contentView = R.layout.accounts
	override val title = R.string.title_activity_accounts

	private var accountList: List<Account> = emptyList()
	private val accountListKey = "accountList"

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (savedInstanceState != null) {
			accountList = savedInstanceState
				.getFromJson(accountListKey, emptyList())

			fillAccounts()
		} else {
			callApi {
				it.listAccounts(this::handleAccounts)
			}
		}
	}

	override fun onSaveInstanceState(outState: Bundle) {
		super.onSaveInstanceState(outState)
		outState.putJson(accountListKey, accountList)
	}

	private fun handleAccounts(accountList: AccountList) {
		this.accountList = accountList.accountList
		fillAccounts()
	}

	private fun fillAccounts() {
		if (accountList.isEmpty()) {
			main_table.visibility = View.GONE
			empty_list.visibility = View.VISIBLE
		} else {
			main_table.visibility = View.VISIBLE
			empty_list.visibility = View.GONE

			main_table.adapter = AccountAdapter(this, accountList)
		}
	}
}
