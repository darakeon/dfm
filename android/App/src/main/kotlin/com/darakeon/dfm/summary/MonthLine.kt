package com.darakeon.dfm.summary

import android.content.Context
import android.util.AttributeSet
import android.widget.LinearLayout
import com.darakeon.dfm.databinding.MonthLineBinding
import com.darakeon.dfm.extract.ExtractActivity
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.lib.extensions.setValueColored

class MonthLine(context: Context, attributeSet: AttributeSet)
	: LinearLayout(context, attributeSet) {
	fun setMonth(
		name: String,
		total: Double,
		accountUrl: String,
		yearNumber: Int,
		monthNumber: Int
	) {
		val binding = MonthLineBinding.bind(this)

		binding.name.text = name

		binding.value.setValueColored(total)

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
