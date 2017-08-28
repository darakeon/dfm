package com.darakeon.dfm.activities.objects

import android.view.LayoutInflater
import org.json.JSONArray

object SummaryStatic : SmartStatic
{
    override var succeeded: Boolean = false
    override var inflater: LayoutInflater? = null

    lateinit var monthList: JSONArray
    var name: String? = null
    var total: Double = 0.toDouble()
    var year: Int = 0
}

