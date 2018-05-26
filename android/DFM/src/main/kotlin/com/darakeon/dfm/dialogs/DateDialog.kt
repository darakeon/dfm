package com.darakeon.dfm.dialogs

import android.app.Activity
import android.app.DatePickerDialog
import android.view.View
import com.darakeon.dfm.extensions.getChildOrMe

fun Activity.getDateDialog(
	getAnswer: (Int, Int, Int) -> Unit,
	year: Int, month: Int? = null, day: Int? = null
) : DatePickerDialog {

	var dialog: DatePickerDialog? = null;

	dialog = DatePickerDialog(
			this,
			{ v, y, m, d ->
				run {
					if (v.isShown) {
						getAnswer(y, m, d)
						dialog?.dismiss()
					}
				}
			},
			year,
			month ?: 1,
			day ?: 1
	)

	val picker = dialog.getChildOrMe("mDatePicker")
	val delegate = picker.getChildOrMe("mDelegate")

	if (day == null) {
		val dayView = delegate.getChildOrMe("mDaySpinner") as View
		dayView.visibility = View.GONE
	}

	if (month == null) {
		val monthView = delegate.getChildOrMe("mMonthSpinner") as View
		monthView.visibility = View.GONE
	}

	return dialog
}
