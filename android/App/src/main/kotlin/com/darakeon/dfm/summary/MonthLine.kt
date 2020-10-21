package com.darakeon.dfm.summary

import android.content.Context
import android.util.AttributeSet
import android.widget.LinearLayout
import com.darakeon.dfm.extract.ExtractActivity
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.lib.extensions.setValueColored
import kotlinx.android.synthetic.main.month_line.view.name
import kotlinx.android.synthetic.main.month_line.view.value

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

		value.setValueColored(total)

		isClickable = true

		setOnLongClickListener {
			context.redirect<ExtractActivity> {
				it.putExtra("accountUrl", accountUrl)
				it.putExtra("year", yearNumber)
				it.putExtra("month", monthNumber - 1)
			}
			true
		}
	}
}
