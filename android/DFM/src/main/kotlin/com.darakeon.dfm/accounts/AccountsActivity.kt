package com.darakeon.dfm.accounts

import android.os.Bundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.accounts.Account
import com.darakeon.dfm.api.entities.accounts.AccountList
import com.darakeon.dfm.api.old.DELETE
import com.darakeon.dfm.auth.auth
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.fromJson
import com.google.gson.Gson
import kotlinx.android.synthetic.main.accounts.empty_list
import kotlinx.android.synthetic.main.accounts.main_table

class AccountsActivity : BaseActivity<DELETE>(DELETE) {
	override val contentView = R.layout.accounts

	private var accountList: Array<Account> = emptyArray()
	private var accountListKey = "accountList"

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		val accountsJson = savedInstanceState?.get(accountListKey)
		if (accountsJson != null) {
			accountList = Gson().fromJson(accountsJson)
			fillAccounts()
		} else {
			api.listAccounts(
				auth,
				this::handleAccounts
			)
		}
	}

	override fun onSaveInstanceState(outState: Bundle) {
		super.onSaveInstanceState(outState)
		val json = Gson().toJson(accountList)
		outState.putCharSequence(accountListKey, json)
	}

	private fun handleAccounts(accountList: AccountList) {
		this.accountList = accountList.list
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
