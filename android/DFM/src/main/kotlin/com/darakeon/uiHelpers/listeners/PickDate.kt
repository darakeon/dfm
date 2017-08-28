package com.dontflymoney.listeners

import android.app.DatePickerDialog
import android.widget.DatePicker

class PickDate(private val activity: IDatePickerActivity) : DatePickerDialog.OnDateSetListener {

    override fun onDateSet(view: DatePicker, year: Int, month: Int, day: Int) {
        if (view.isShown) {
            activity.setResult(year, month, day)
            activity.dialog?.dismiss()
        }
    }
}
