package com.darakeon.dfm.summary

import com.darakeon.dfm.api.entities.summary.Month
import com.darakeon.dfm.base.SmartStatic
import org.json.JSONArray

object SummaryStatic : SmartStatic
{
	override var succeeded: Boolean = false

	var monthList: Array<Month> = emptyArray()
	var name: String? = null
	var total: Double = 0.toDouble()
	var year: Int = 0
}

