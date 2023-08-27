package com.darakeon.dfm.summary

import android.os.Bundle
import android.view.View
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.SummaryBinding
import com.darakeon.dfm.dialogs.getDateDialog
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.summary.Summary
import com.darakeon.dfm.lib.extensions.Direction
import com.darakeon.dfm.lib.extensions.setValueColored
import com.darakeon.dfm.lib.extensions.swipe
import kotlinx.android.synthetic.main.summary.empty_list
import kotlinx.android.synthetic.main.summary.main
import kotlinx.android.synthetic.main.summary.main_table
import kotlinx.android.synthetic.main.summary.reportChange
import kotlinx.android.synthetic.main.summary.total_title
import kotlinx.android.synthetic.main.summary.total_value
import java.util.Calendar

class SummaryActivity : BaseActivity<SummaryBinding>() {
	private var accountUrl: String = ""

	private var year: Int = 0
	private val yearKey = "year"

	private var summary = Summary()
	private val summaryKey = "summary"

	override val contentViewId = R.layout.summary
	override val title = R.string.title_activity_summary

	override val refresh: SwipeRefreshLayout?
		get() = main

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		accountUrl = getExtraOrUrl("accountUrl") ?: ""

		if (savedInstanceState != null) {
			val year = savedInstanceState.getInt(yearKey)
			setDate(year)

			summary = savedInstanceState.getFromJson(summaryKey, Summary())
			fillSummary()
		} else {
			setDate(getYear())
			getSummary()
		}
	}

	private fun getYear(): Int {
		val today = Calendar.getInstance()
		return query["id"]?.toIntOrNull()
			?: intent.getIntExtra("year", today[Calendar.YEAR])
	}

	private fun setDate(year: Int) {
		this.year = year
		intent.putExtra(yearKey, year)
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
		total_value.setValueColored(summary.total)

		if (summary.monthList.isEmpty()) {
			main_table.visibility = View.GONE
			empty_list.visibility = View.VISIBLE

			main.listChild = main_table
			// TODO: test it
			main.swipe(Direction.Right, this::past)
			main.swipe(Direction.Left, this::future)
		} else {
			main_table.visibility = View.VISIBLE
			empty_list.visibility = View.GONE

			main_table.adapter = MonthAdapter(
				this,
				summary.monthList,
				accountUrl,
				year
			)

			// TODO: test it
			main_table.swipe(Direction.Right, this::past)
			main_table.swipe(Direction.Left, this::future)
		}
	}

	private fun past() {
		getSummary(year-1)
	}

	private fun future() {
		getSummary(year+1)
	}

	fun changeDate(@Suppress(ON_CLICK) view: View) {
		when (view.id) {
			R.id.prev -> past()
			R.id.next -> future()
			else ->
				getDateDialog(
					year, this::getSummary
				).show()
		}
	}

	private fun getSummary(newYear: Int) {
		setDate(newYear)
		getSummary()
	}

	override fun onSaveInstanceState(outState: Bundle) {
		super.onSaveInstanceState(outState)

		outState.putJson(summaryKey, summary)
		outState.putInt(yearKey, year)
	}
}
