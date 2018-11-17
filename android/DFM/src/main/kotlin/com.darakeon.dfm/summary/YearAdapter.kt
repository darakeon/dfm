package com.darakeon.dfm.summary

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.summary.Month
import com.darakeon.dfm.auth.getThemeLineColor

class YearAdapter(context: Context, monthList: Array<Month>, accountUrl: String, yearNumber: Int) : BaseAdapter() {

	private val monthList: MutableList<CompleteMonth> =
		monthList.map { CompleteMonth(it, accountUrl, yearNumber) }
				.toMutableList()

	private val inflater = context.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

	inner class CompleteMonth(month: Month, var accountUrl: String, var yearNumber: Int) {
		var monthName: String = month.name
		var monthNumber: Int = month.number
		var total: Double = month.total
	}

	override fun getCount(): Int = monthList.size
	override fun getItem(position: Int): Any = position
	override fun getItemId(position: Int): Long = position.toLong()

	override fun getView(position: Int, view: View?, viewGroup: ViewGroup): View? {
		val line = inflater.inflate(R.layout.summary_line, null) as YearLine

		val color = getThemeLineColor(position)
		line.setYear(monthList[position], color)

		return line
	}

}
