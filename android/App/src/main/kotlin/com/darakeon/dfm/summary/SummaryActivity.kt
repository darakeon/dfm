package com.darakeon.dfm.summary

import android.os.Bundle
import android.view.View
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.BottomMenuBinding
import com.darakeon.dfm.databinding.SummaryBinding
import com.darakeon.dfm.dialogs.getDateDialog
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.summary.Summary
import com.darakeon.dfm.lib.extensions.Direction
import com.darakeon.dfm.lib.extensions.setValueColored
import com.darakeon.dfm.lib.extensions.swipe
import java.util.Calendar

class SummaryActivity : BaseActivity<SummaryBinding>() {
	override fun inflateBinding(): SummaryBinding {
		return SummaryBinding.inflate(layoutInflater)
	}
	override fun getMenuBinding(): BottomMenuBinding {
		return binding.bottomMenu
	}

	private var accountUrl: String = ""

	private var year: Int = 0
	private val yearKey = "year"

	private var summary = Summary()
	private val summaryKey = "summary"

	override val title = R.string.title_activity_summary

	override val refresh: SwipeRefreshLayout
		get() = binding.main

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
		binding.reportChange.text = year.toString()
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
		binding.totalTitle.text = summary.title
		binding.totalValue.setValueColored(summary.total)

		if (summary.monthList.isEmpty()) {
			binding.mainTable.visibility = View.GONE
			binding.emptyList.visibility = View.VISIBLE

			binding.main.listChild = binding.mainTable
			// TODO: test it
			binding.main.swipe(Direction.Right, this::past)
			binding.main.swipe(Direction.Left, this::future)
		} else {
			binding.mainTable.visibility = View.VISIBLE
			binding.emptyList.visibility = View.GONE

			binding.mainTable.adapter = MonthAdapter(
				this,
				summary.monthList,
				accountUrl,
				year
			)

			// TODO: test it
			binding.mainTable.swipe(Direction.Right, this::past)
			binding.mainTable.swipe(Direction.Left, this::future)
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
