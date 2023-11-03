package com.darakeon.dfm.accounts

import android.os.Bundle
import android.view.View
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.AccountsBinding
import com.darakeon.dfm.databinding.BottomMenuBinding
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.accounts.Account
import com.darakeon.dfm.lib.api.entities.accounts.AccountList

class AccountsActivity : BaseActivity<AccountsBinding>() {
	override fun inflateBinding(): AccountsBinding {
		return AccountsBinding.inflate(layoutInflater)
	}
	override fun getMenuBinding(): BottomMenuBinding {
		return binding.bottomMenu
	}

	override val title = R.string.title_activity_accounts

	private var accountList: List<Account> = emptyList()
	private val accountListKey = "accountList"

	override val refresh: SwipeRefreshLayout
		get() = binding.main

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
			binding.mainTable.visibility = View.GONE
			binding.emptyList.visibility = View.VISIBLE
		} else {
			binding.mainTable.visibility = View.VISIBLE
			binding.emptyList.visibility = View.GONE

			binding.main.listChild = binding.mainTable
			binding.mainTable.adapter = AccountAdapter(this, accountList)
		}
	}
}
