package com.darakeon.dfm.activities

import android.app.DatePickerDialog
import android.os.Bundle
import android.view.View
import android.widget.ListView
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.*
import com.darakeon.dfm.activities.objects.SummaryStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.uiHelpers.adapters.YearAdapter
import com.darakeon.dfm.user.getAuth
import org.json.JSONObject
import java.util.*

class SummaryActivity : SmartActivity<SummaryStatic>(SummaryStatic) {

	private val main: ListView get() = findViewById(R.id.main_table)
	private val empty: TextView get() = findViewById(R.id.empty_list)

	private val accountUrl: String get() = getExtraOrUrl("accountUrl")

	private val dialog: DatePickerDialog
		get() = getDateDialog(
			{ y, _, _ -> updateScreen(y) },
			static.year
		)

	override fun contentView(): Int = R.layout.summary

	private fun updateScreen(year: Int) {
		setDate(year)
		getSummary()
	}

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (rotated && succeeded) {
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
		setValue(R.id.reportChange, Integer.toString(year))
	}

	fun changeDate(@Suppress(ON_CLICK) view: View) {
		dialog.show()
	}

	private fun getSummary() {
		val request = InternalRequest(
			this, "Moves/Summary", { d -> handleSummary(d) }
		)

		request.addParameter("ticket", getAuth())
		request.addParameter("accountUrl", accountUrl)
		request.addParameter("id", static.year)

		request.post()
	}

	private fun handleSummary(data: JSONObject) {
		static.monthList = data.getJSONArray("MonthList")
		static.name = data.getString("Name")
		static.total = data.getDouble("Total")

		fillSummary()
	}

	private fun fillSummary() {
		setValue(R.id.totalTitle, static.name)
		setValueColored(R.id.totalValue, static.total)

		if (static.monthList.length() == 0) {
			main.visibility = View.GONE
			empty.visibility = View.VISIBLE
		} else {
			main.visibility = View.VISIBLE
			empty.visibility = View.GONE

			val yearAdapter = YearAdapter(this, static.monthList, accountUrl, static.year)
			main.adapter = yearAdapter
		}

	}
}

