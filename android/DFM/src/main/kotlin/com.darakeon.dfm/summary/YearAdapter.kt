package com.darakeon.dfm.summary

import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.summary.Month
import com.darakeon.dfm.base.Adapter

class YearAdapter(
	activity: SummaryActivity,
	monthList: Array<Month>,
	accountUrl: String,
	yearNumber: Int
) : Adapter<SummaryActivity, Month, YearLine>(activity, monthList) {
	private val monthList: MutableList<CompleteMonth> =
		monthList.map {
			CompleteMonth(it, accountUrl, yearNumber)
		}.toMutableList()

	override val id: Int
		get() = R.layout.summary_line

	override fun populateView(view: YearLine, position: Int) =
		view.setYear(monthList[position])

	inner class CompleteMonth(month: Month, var accountUrl: String, var yearNumber: Int) {
		var monthName: String = month.name
		var monthNumber: Int = month.number
		var total: Double = month.total
	}
}
