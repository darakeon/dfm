package com.darakeon.dfm.dialogs

import android.app.Activity
import android.app.DatePickerDialog
import android.view.View
import android.widget.DatePicker
import com.darakeon.dfm.lib.extensions.getPrivate

fun Activity.getDateDialog(year: Int, getAnswer: (Int) -> Unit) =
	getDateDialog(year, null) {
		y, _ -> getAnswer(y)
	}.hide("Month")

fun Activity.getDateDialog(year: Int, month: Int?, getAnswer: (Int, Int) -> Unit) =
	getDateDialog(year, month, null) {
		y, m, _ -> getAnswer(y, m)
	}.hide("Day")

fun Activity.getDateDialog(
	year: Int, month: Int?, day: Int?,
	getAnswer: (Int, Int, Int) -> Unit
) = DatePickerDialog(
	this,
	{
		picker, newYear, newMonth, newDay ->
			onSet(picker, newYear, newMonth, newDay, getAnswer)
	},
	year,
	month ?: 1,
	day ?: 1
)

private fun onSet(
	picker: DatePicker,
	year: Int,
	month: Int,
	day: Int,
	getAnswer: (Int, Int, Int) -> Unit
) {
	if (picker.isShown) {
		getAnswer(year, month, day)
	}
}

private fun DatePickerDialog.hide(fieldName: String): DatePickerDialog {
	val child = getPrivate<View>(
		"mDatePicker",
		"mDelegate",
		"m${fieldName}Spinner"
	)

	child.visibility = View.GONE
	return this
}
