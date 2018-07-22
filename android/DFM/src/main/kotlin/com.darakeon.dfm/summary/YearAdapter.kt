package com.darakeon.dfm.summary

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import com.darakeon.dfm.R
import com.darakeon.dfm.auth.getThemeLineColor
import org.json.JSONArray
import org.json.JSONObject

class YearAdapter(context: Context, yearJsonList: JSONArray, accountUrl: String, yearNumber: Int) : BaseAdapter() {

	private val yearList: MutableList<Year> =
		(0 until yearJsonList.length())
			.map { Year(yearJsonList.getJSONObject(it), accountUrl, yearNumber) }
			.toMutableList()

	private val inflater = context.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

	inner class Year(jsonObject: JSONObject, var AccountUrl: String, var YearNumber: Int) {
		var monthName: String = jsonObject.getString("Name")
		var monthNumber: Int = jsonObject.getInt("Number")
		var total: Double = jsonObject.getDouble("Total")
	}

	override fun getCount(): Int = yearList.size
	override fun getItem(position: Int): Any = position
	override fun getItemId(position: Int): Long = position.toLong()

	override fun getView(position: Int, view: View?, viewGroup: ViewGroup): View? {
		val line = inflater.inflate(R.layout.summary_line, null) as YearLine

		val color = getThemeLineColor(position)
		line.setYear(yearList[position], color)

		return line
	}

}
