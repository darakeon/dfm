package com.dontflymoney.adapters

import android.annotation.SuppressLint
import android.content.Context
import android.graphics.Color
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter

import com.dontflymoney.layout.YearLine
import com.dontflymoney.view.R

import org.json.JSONArray
import org.json.JSONException
import org.json.JSONObject

import java.util.ArrayList

class YearAdapter @Throws(JSONException::class)
constructor(context: Context, yearJsonList: JSONArray, accountUrl: String, yearNumber: Int) : BaseAdapter() {
    private val yearList: MutableList<Year>

    init {
        yearList = ArrayList<Year>()

        for (a in 0..yearJsonList.length() - 1) {
            val year = Year(yearJsonList.getJSONObject(a), accountUrl, yearNumber)
            yearList.add(year)
        }

        inflater = context.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater
    }

    inner class Year @Throws(JSONException::class)
    internal constructor(jsonObject: JSONObject, var AccountUrl: String, var YearNumber: Int) {
        var MonthName: String
        var MonthNumber: Int = 0

        var Total: Double = 0.toDouble()

        init {
            MonthName = jsonObject.getString("Name")
            MonthNumber = jsonObject.getInt("Number")
            Total = jsonObject.getDouble("Total")
        }
    }

    override fun getCount(): Int {
        return yearList.size
    }

    override fun getItem(position: Int): Any {
        return position
    }

    override fun getItemId(position: Int): Long {
        return position.toLong()
    }

    override fun getView(position: Int, view: View, viewGroup: ViewGroup): View {
        @SuppressLint("ViewHolder", "InflateParams")
        val line = inflater!!.inflate(R.layout.summary_line, null) as YearLine

        try {
            val color = if (position % 2 == 0) Color.TRANSPARENT else Color.LTGRAY
            line.setYear(yearList[position], color)
        } catch (e: JSONException) {
            e.printStackTrace()
        }

        return line
    }

    companion object {
        private var inflater: LayoutInflater? = null
    }

}
