package com.dontflymoney.activityObjects

import android.view.LayoutInflater
import com.dontflymoney.entities.Move
import org.json.JSONArray

object MovesCreateStatic : SmartStatic
{
    override var succeeded: Boolean = false
    override var inflater: LayoutInflater? = null

    var move: Move = Move()
    var useCategories: Boolean = false
    var categoryList: JSONArray? = null
    var natureList: JSONArray? = null
    var accountList: JSONArray? = null
}

