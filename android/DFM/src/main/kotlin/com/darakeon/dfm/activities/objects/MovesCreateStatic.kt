package com.darakeon.dfm.activities.objects

import android.view.LayoutInflater
import com.darakeon.dfm.api.entities.Move
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

