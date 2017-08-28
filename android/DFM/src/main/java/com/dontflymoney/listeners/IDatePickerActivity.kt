package com.dontflymoney.listeners

import android.app.DatePickerDialog

interface IDatePickerActivity {
    fun setResult(year: Int, month: Int, day: Int)
    var dialog: DatePickerDialog?
}
