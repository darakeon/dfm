package com.darakeon.dfm.activities.objects

import org.json.JSONArray

object AccountsStatic : SmartStatic
{
	override var succeeded: Boolean = false

	lateinit var accountList: JSONArray
}

