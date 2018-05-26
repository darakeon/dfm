package com.darakeon.dfm.summary

import android.app.DatePickerDialog
import android.os.Bundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.auth.getAuth
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
import org.json.JSONObject
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
		total_title.text = static.name
		setValueColored(total_value, static.total)

		if (static.monthList.length() == 0) {
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

