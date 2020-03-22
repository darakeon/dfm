package com.darakeon.dfm.summary

import android.content.Context
import android.util.AttributeSet
import android.widget.LinearLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.extensions.setColorByAttr
import com.darakeon.dfm.extract.ExtractActivity
import kotlinx.android.synthetic.main.month_line.view.name
import kotlinx.android.synthetic.main.month_line.view.value
import java.text.DecimalFormat

class MonthLine(context: Context, attributeSet: AttributeSet)
	: LinearLayout(context, attributeSet) {
	fun setMonth(
		name: String,
		total: Double,
		accountUrl: String,
		yearNumber: Int,
		monthNumber: Int
	) {
		this.name.text = name

		val totalColor = if (total < 0) R.attr.negative else R.attr.positive
		val totalToShow = if (total < 0) -total else total
		val totalStr = DecimalFormat("#,##0.00").format(totalToShow)

		value.setColorByAttr(totalColor)
		value.text = totalStr

		isClickable = true

		setOnClickListener {
			context.redirect<ExtractActivity> {
				it.putExtra("accountUrl", accountUrl)
				it.putExtra("year", yearNumber)
				it.putExtra("month", monthNumber - 1)
			}
		}
	}
}
