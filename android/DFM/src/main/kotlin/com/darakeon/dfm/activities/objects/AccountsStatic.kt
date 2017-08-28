package com.darakeon.dfm.activities.objects

import android.view.LayoutInflater
import org.json.JSONArray

object AccountsStatic : SmartStatic
{
	override var succeeded: Boolean = false
	override var inflater: LayoutInflater? = null

	lateinit var accountList: JSONArray

}

