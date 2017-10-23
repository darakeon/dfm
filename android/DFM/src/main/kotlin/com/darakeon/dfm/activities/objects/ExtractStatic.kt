package com.darakeon.dfm.activities.objects

import org.json.JSONArray

object ExtractStatic : SmartStatic
{
	override var succeeded: Boolean = false

	lateinit var moveList: JSONArray
	var name: String? = null
	var total: Double = 0.toDouble()
	var canCheck: Boolean = false
	var month: Int = 0
	var year: Int = 0
}

