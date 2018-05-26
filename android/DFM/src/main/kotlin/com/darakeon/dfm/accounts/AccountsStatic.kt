package com.darakeon.dfm.accounts

import com.darakeon.dfm.base.SmartStatic
import org.json.JSONArray

object AccountsStatic : SmartStatic
{
	override var succeeded: Boolean = false

	lateinit var accountList: JSONArray
}

