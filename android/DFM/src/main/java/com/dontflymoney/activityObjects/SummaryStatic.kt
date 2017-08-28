package com.dontflymoney.activityObjects

import android.view.LayoutInflater
import org.json.JSONArray

object SummaryStatic : SmartStatic
{
    override var succeeded: Boolean = false
    override var inflater: LayoutInflater? = null

    var monthList: JSONArray? = null
    var name: String? = null
    var total: Double = 0.toDouble()
    var year: Int = 0
}

