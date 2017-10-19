package com.darakeon.dfm.uiHelpers.views

import android.content.Context
import android.content.Intent
import android.util.AttributeSet
import android.view.View
import android.widget.LinearLayout
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.ExtractActivity
import com.darakeon.dfm.activities.base.setColorByAttr
import com.darakeon.dfm.uiHelpers.adapters.YearAdapter
import java.text.DecimalFormat

class YearLine(context: Context, attributeSet: AttributeSet) : LinearLayout(context, attributeSet) {

	private val monthField: TextView get() = findViewById(R.id.month)
	private val totalField: TextView get() = findViewById(R.id.value)

	fun setYear(year: YearAdapter.Year, color: Int) {
		setBackgroundColor(color)

		monthField.text = year.monthName

		val totalColor = if (year.total < 0) R.attr.negative else R.attr.positive
		val totalToShow = if (year.total < 0) -year.total else year.total
		val totalStr = DecimalFormat("#,##0.00").format(totalToShow)

		totalField.setColorByAttr(totalColor)
		totalField.text = totalStr

		isClickable = true

		setOnClickListener(OnClickListener(context, year))
	}

	inner class OnClickListener internal constructor(
		private val context: Context,
		private val year: YearAdapter.Year
	) : View.OnClickListener {

		override fun onClick(v: View) {
			val intent = Intent(context, ExtractActivity::class.java)

			intent.putExtra("accountUrl", year.AccountUrl)
			intent.putExtra("year", year.YearNumber)
			intent.putExtra("month", year.monthNumber - 1)

			context.startActivity(intent)
		}
	}


}
