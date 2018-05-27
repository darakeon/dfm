package com.darakeon.dfm.accounts

import com.darakeon.dfm.api.Account
import com.darakeon.dfm.base.SmartStatic

object AccountsStatic : SmartStatic
{
	override var succeeded: Boolean = false

	lateinit var accountList: List<Account>
}

