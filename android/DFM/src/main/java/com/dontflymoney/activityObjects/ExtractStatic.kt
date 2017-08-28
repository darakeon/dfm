package com.dontflymoney.activityObjects

import android.view.LayoutInflater
import org.json.JSONArray

object ExtractStatic : SmartStatic
{
    override var succeeded: Boolean = false
    override var inflater: LayoutInflater? = null

    var moveList: JSONArray? = null
    var name: String? = null
    var total: Double = 0.toDouble()
    var canCheck: Boolean = false
    var month: Int = 0
    var year: Int = 0

}

