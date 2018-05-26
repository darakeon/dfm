package com.darakeon.dfm.accounts

import android.os.Bundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.auth.getAuth
import com.darakeon.dfm.base.SmartActivity
import kotlinx.android.synthetic.main.accounts.empty_list
import kotlinx.android.synthetic.main.accounts.main_table
import org.json.JSONObject

class AccountsActivity : SmartActivity<AccountsStatic>(AccountsStatic) {
	override fun contentView(): Int = R.layout.accounts

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (rotated && succeeded) {
			fillAccounts()
		} else {
			getAccounts()
		}
	}

	private fun getAccounts() {
		val request = InternalRequest(
			this, "Accounts/List", { d -> handleAccounts(d) }
		)
		request.addParameter("ticket", getAuth())
		request.post()
	}

	private fun handleAccounts(data: JSONObject) {
		static.accountList = data.getJSONArray("AccountList")
		fillAccounts()
	}

	private fun fillAccounts() {
		if (static.accountList.length() == 0) {
			main_table.visibility = View.GONE
			empty_list.visibility = View.VISIBLE
		} else {
			main_table.visibility = View.VISIBLE
			empty_list.visibility = View.GONE

			main_table.adapter = AccountAdapter(this, static.accountList)
		}
	}


}
