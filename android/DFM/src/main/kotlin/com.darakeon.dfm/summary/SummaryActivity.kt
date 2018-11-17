package com.darakeon.dfm.summary

import android.app.DatePickerDialog
import android.os.Bundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.summary.Summary
import com.darakeon.dfm.auth.auth
import com.darakeon.dfm.auth.highLightColor
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.dialogs.getDateDialog
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.setValueColored
import kotlinx.android.synthetic.main.summary.empty_list
import kotlinx.android.synthetic.main.summary.main_table
import kotlinx.android.synthetic.main.summary.reportChange
import kotlinx.android.synthetic.main.summary.total_title
import kotlinx.android.synthetic.main.summary.total_value
import java.util.Calendar

class SummaryActivity : BaseActivity<SummaryStatic>(SummaryStatic) {

	private val accountUrl: String get() = getExtraOrUrl("accountUrl")

	private val dialog: DatePickerDialog
		get() = getDateDialog(
			{ y, _, _ -> updateScreen(y) },
			static.year
		)

	override val contentView = R.layout.summary

	private fun updateScreen(year: Int) {
		setDate(year)
		getSummary()
	}

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		highlight?.setBackgroundColor(highLightColor)

		if (rotated && static.succeeded) {
			setDateFromLast()
			fillSummary()
		} else {
			setDateFromCaller()
			getSummary()
		}
	}

	private fun setDateFromLast() {
		setDate(static.year)
	}

	private fun setDateFromCaller() {
		if (query.containsKey("id")) {
			val startYear = query["id"]?.toInt() ?: 0
			setDate(startYear)
		} else {
			val today = Calendar.getInstance()
			val startYear = intent.getIntExtra("year", today.get(Calendar.YEAR))
			setDate(startYear)
		}
	}

	private fun setDate(year: Int) {
		static.year = year
		reportChange.text = year.toString()
	}

	fun changeDate(@Suppress(ON_CLICK) view: View) {
		dialog.show()
	}

	private fun getSummary() {
		api.getSummary(auth, accountUrl, static.year, this::handleSummary)
	}

	private fun handleSummary(data: Summary) {
		static.monthList = data.monthList
		static.name = data.name
		static.total = data.total

		fillSummary()
	}

	private fun fillSummary() {
		total_title.text = static.name
		setValueColored(total_value, static.total)

		if (static.monthList.isEmpty()) {
			main_table.visibility = View.GONE
			empty_list.visibility = View.VISIBLE
		} else {
			main_table.visibility = View.VISIBLE
			empty_list.visibility = View.GONE

			val yearAdapter = YearAdapter(this, static.monthList, accountUrl, static.year)
			main_table.adapter = yearAdapter
		}

	}
}

