package com.darakeon.dfm.activities.objects

import com.darakeon.dfm.api.entities.Move
import org.json.JSONArray

object MovesCreateStatic : SmartStatic
{
	override var succeeded: Boolean = false

	var move: Move = Move()
	var useCategories: Boolean = false
	var categoryList: JSONArray = JSONArray()
	var natureList: JSONArray = JSONArray()
	var accountList: JSONArray = JSONArray()
}

