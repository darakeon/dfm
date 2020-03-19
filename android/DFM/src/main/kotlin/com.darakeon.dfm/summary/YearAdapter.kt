package com.darakeon.dfm.summary

import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.summary.Month
import com.darakeon.dfm.base.Adapter

class YearAdapter(
	activity: SummaryActivity,
	monthList: Array<Month>,
	accountUrl: String,
	yearNumber: Int
) : Adapter<SummaryActivity, Month, MonthLine>(activity, monthList) {
	private val monthList: MutableList<CompleteMonth> =
		monthList.map {
			CompleteMonth(it, accountUrl, yearNumber)
		}.toMutableList()

	override val id: Int
		get() = R.layout.month_line

	override fun populateView(view: MonthLine, position: Int) =
		monthList[position].populate(view)

	inner class CompleteMonth(month: Month, var accountUrl: String, var yearNumber: Int) {
		fun populate(view: MonthLine) {
			view.setMonth(
				name, total, accountUrl, yearNumber, monthNumber
			)
		}

		var name = month.name
		var total = month.total
		var monthNumber = month.number
	}
}
