package com.darakeon.dfm.extract

import com.darakeon.dfm.api.entities.Move
import com.darakeon.dfm.base.SmartStatic
import org.json.JSONArray

object ExtractStatic : SmartStatic
{
	override var succeeded: Boolean = false

	lateinit var moveList: Array<Move>
	var name: String? = null
	var total: Double = 0.toDouble()
	var canCheck: Boolean = false
	var month: Int = 0
	var year: Int = 0
}

