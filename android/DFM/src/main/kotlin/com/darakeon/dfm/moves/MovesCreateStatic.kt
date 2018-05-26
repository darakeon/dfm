package com.darakeon.dfm.moves

import com.darakeon.dfm.base.SmartStatic
import com.darakeon.dfm.api.old.entities.Move
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

