package com.darakeon.dfm.summary

import android.app.DatePickerDialog
import android.os.Bundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.summary.Summary
import com.darakeon.dfm.auth.highLightColor
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.dialogs.getDateDialog
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.extensions.setValueColored
import kotlinx.android.synthetic.main.summary.empty_list
import kotlinx.android.synthetic.main.summary.main_table
import kotlinx.android.synthetic.main.summary.reportChange
import kotlinx.android.synthetic.main.summary.total_title
import kotlinx.android.synthetic.main.summary.total_value
import java.util.Calendar

class SummaryActivity : BaseActivity() {
	private val accountUrl: String get() = getExtraOrUrl("accountUrl")

	private var year: Int = 0
	private val yearKey = "yearKey"

	private var summary = Summary()
	private val summaryKey = "summaryKey"

	private val dialog: DatePickerDialog
		get() = getDateDialog(
			{ y, _, _ -> updateScreen(y) },
			year
		)

	override val contentView = R.layout.summary
	override val title = R.string.title_activity_summary

	private fun updateScreen(year: Int) {
		setDate(year)
		getSummary()
	}

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		highlight?.setBackgroundColor(highLightColor)

		if (savedInstanceState != null) {
			year = savedInstanceState.getInt(yearKey)
			summary = savedInstanceState.getFromJson(summaryKey, Summary())

			setDate(year)
			fillSummary()
		} else {
			setDateFromCaller()
			getSummary()
		}
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
		this.year = year
		reportChange.text = year.toString()
	}

	private fun getSummary() {
		callApi { api ->
			api.getSummary(accountUrl, year) {
				summary = it
				fillSummary()
			}
		}
	}

	private fun fillSummary() {
		total_title.text = summary.title
		setValueColored(total_value, summary.total)

		if (summary.monthList.isEmpty()) {
			main_table.visibility = View.GONE
			empty_list.visibility = View.VISIBLE
		} else {
			main_table.visibility = View.VISIBLE
			empty_list.visibility = View.GONE

			val yearAdapter = YearAdapter(this, summary.monthList, accountUrl, year)
			main_table.adapter = yearAdapter
		}
	}

	fun changeDate(@Suppress(ON_CLICK) view: View) {
		dialog.show()
	}

	override fun onSaveInstanceState(outState: Bundle) {
		super.onSaveInstanceState(outState)

		outState.putJson(summaryKey, summary)
		outState.putInt(yearKey, year)
	}
}
