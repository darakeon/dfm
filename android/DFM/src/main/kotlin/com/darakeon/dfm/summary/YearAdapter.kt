package com.darakeon.dfm.summary

import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.summary.Month
import com.darakeon.dfm.base.Adapter

class YearAdapter(
	activity: SummaryActivity,
	list: Array<Month>,
	private val accountUrl: String,
	private val yearNumber: Int
) : Adapter<SummaryActivity, Month, MonthLine>(activity, list) {
	override val id: Int
		get() = R.layout.month_line

	override fun populateView(view: MonthLine, position: Int) {
		val month = list[position]

		view.setMonth(
			month.name, month.total,
			accountUrl, yearNumber, month.number
		)
	}
}
