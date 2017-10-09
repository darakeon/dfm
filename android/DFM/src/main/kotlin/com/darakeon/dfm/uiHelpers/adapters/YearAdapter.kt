package com.darakeon.dfm.uiHelpers.adapters

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import com.darakeon.dfm.R
import com.darakeon.dfm.uiHelpers.views.YearLine
import com.darakeon.dfm.user.getThemeLineColor
import org.json.JSONArray
import org.json.JSONObject

class YearAdapter(context: Context, yearJsonList: JSONArray, accountUrl: String, yearNumber: Int) : BaseAdapter() {

	private val yearList: MutableList<Year> =
		(0..yearJsonList.length() - 1)
			.map { Year(yearJsonList.getJSONObject(it), accountUrl, yearNumber) }
			.toMutableList()

	private val inflater = context.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

	inner class Year(jsonObject: JSONObject, var AccountUrl: String, var YearNumber: Int) {
		var MonthName: String = jsonObject.getString("Name")
		var MonthNumber: Int = jsonObject.getInt("Number")
		var Total: Double = jsonObject.getDouble("Total")
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

	override fun getView(position: Int, view: View?, viewGroup: ViewGroup): View? {
		val line = inflater.inflate(R.layout.summary_line, null) as YearLine

		val color = getThemeLineColor(position)
		line.setYear(yearList[position], color)

		return line
	}

}
