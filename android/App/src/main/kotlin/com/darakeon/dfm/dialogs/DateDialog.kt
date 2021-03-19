package com.darakeon.dfm.dialogs

import android.app.Activity
import android.app.DatePickerDialog
import android.content.res.Resources
import android.view.View
import android.widget.DatePicker
import com.darakeon.dfm.lib.R

fun Activity.getDateDialog(year: Int, getAnswer: (Int) -> Unit) =
	getDateDialog(year, null, null, true) {
		y, _, _ -> getAnswer(y)
	}

fun Activity.getDateDialog(year: Int, month: Int?, getAnswer: (Int, Int) -> Unit) =
	getDateDialog(year, month, null, true) {
		y, m, _ -> getAnswer(y, m)
	}

fun Activity.getDateDialog(
	year: Int, month: Int?, day: Int?,
	getAnswer: (Int, Int, Int) -> Unit,
) = getDateDialog(year, month, day, false, getAnswer)

private fun Activity.getDateDialog(
	year: Int, month: Int?, day: Int?,
	spinner: Boolean,
	getAnswer: (Int, Int, Int) -> Unit,
): DatePickerDialog {
	val action = {
		picker: DatePicker, newYear: Int, newMonth: Int, newDay: Int ->
			onSet(picker, newYear, newMonth, newDay, getAnswer)
	}

	val theme = if (spinner)
		R.style.DateSpinner
	else
		R.style.DateCalendar

	val dialog = DatePickerDialog(
		this, theme, action, year, month ?: 1, day ?: 1
	)

	if (day == null)
		dialog.hide("day")

	if (month == null)
		dialog.hide("month")

	return dialog
}

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
	val id = Resources.getSystem()
		.getIdentifier(fieldName, "id", "android")

	val child = datePicker.findViewById<View>(id)
	child.visibility = View.GONE

	return this
}
