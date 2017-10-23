package com.darakeon.dfm.activities.objects

import org.json.JSONArray

object SummaryStatic : SmartStatic
{
	override var succeeded: Boolean = false

	lateinit var monthList: JSONArray
	var name: String? = null
	var total: Double = 0.toDouble()
	var year: Int = 0
}

