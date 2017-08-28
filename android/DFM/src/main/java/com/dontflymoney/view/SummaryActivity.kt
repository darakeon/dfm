package com.dontflymoney.view

import android.app.DatePickerDialog
import android.os.Bundle
import android.view.View
import android.widget.DatePicker
import android.widget.ListView
import android.widget.TextView

import com.dontflymoney.adapters.YearAdapter
import com.dontflymoney.api.InternalRequest
import com.dontflymoney.api.Step
import com.dontflymoney.baseactivity.SmartActivity
import com.dontflymoney.listeners.IDatePickerActivity
import com.dontflymoney.listeners.PickDate

import org.json.JSONArray
import org.json.JSONException
import org.json.JSONObject

import java.lang.reflect.Field
import java.util.Calendar

class SummaryActivity : SmartActivity(), IDatePickerActivity {

    internal var main: ListView
    internal var empty: TextView

    internal var accountUrl: String

    internal var dialog: DatePickerDialog? = null


    override fun contentView(): Int {
        return R.layout.summary
    }

    override fun optionsMenuResource(): Int {
        return R.menu.summary
    }


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setCurrentInfo()

        if (rotated && SmartActivity.succeded) {
            setDateFromLast()

            try {
                fillSummary()
            } catch (e: JSONException) {
                message.alertError(R.string.error_activity_json, e)
            }

        } else {
            setDateFromCaller()
            getSummary()
        }
    }

    private fun setCurrentInfo() {
        main = findViewById(R.id.main_table) as ListView
        empty = findViewById(R.id.empty_list) as TextView

        accountUrl = intent.getStringExtra("accountUrl")
    }

    private fun setDateFromLast() {
        setDate(year)
    }

    private fun setDateFromCaller() {
        val today = Calendar.getInstance()

        val startYear = intent.getIntExtra("year", today.get(Calendar.YEAR))
        setDate(startYear)
    }

    private fun setDate(year: Int) {
        SummaryActivity.year = year
        form.setValue(R.id.reportDate, Integer.toString(year))
    }

    fun changeDate(v: View) {
        if (dialog == null) {
            dialog = DatePickerDialog(this, PickDate(this), year, 1, 1)

            try {
                val pickerField = dialog!!.javaClass.getDeclaredField("mDatePicker")
                pickerField.setAccessible(true)

                val dayField = pickerField.get(dialog).javaClass.getDeclaredField("mDaySpinner")
                dayField.setAccessible(true)
                val dayPicker = dayField.get(pickerField.get(dialog))
                (dayPicker as View).visibility = View.GONE

                val monthField = pickerField.get(dialog).javaClass.getDeclaredField("mMonthSpinner")
                monthField.setAccessible(true)
                val monthPicker = monthField.get(pickerField.get(dialog))
                (monthPicker as View).visibility = View.GONE
            } catch (ignored: Exception) {
            }

        }

        dialog!!.show()
    }

    override fun setResult(year: Int, month: Int, day: Int) {
        setDate(year)
        getSummary()
    }

    private fun getSummary() {
        val accountUrl = intent.getStringExtra("accountUrl")

        request = InternalRequest(this, "Moves/Summary")

        request.AddParameter("ticket", Authentication.Get())
        request.AddParameter("accountUrl", accountUrl)
        request.AddParameter("id", year)

        request.Post()
    }

    override fun getDialog(): DatePickerDialog {
        return dialog
    }

    @Throws(JSONException::class)
    override fun HandleSuccess(data: JSONObject, step: Step) {
        monthList = data.getJSONArray("MonthList")
        name = data.getString("Name")
        total = data.getDouble("Total")

        fillSummary()
    }

    @Throws(JSONException::class)
    private fun fillSummary() {
        form.setValue(R.id.totalTitle, name)
        form.setValueColored(R.id.totalValue, total)

        if (monthList.length() == 0) {
            main.visibility = View.GONE
            empty.visibility = View.VISIBLE
        } else {
            main.visibility = View.VISIBLE
            empty.visibility = View.GONE

            val yearAdapter = YearAdapter(this, monthList, accountUrl, year)
            main.adapter = yearAdapter
        }

    }

    companion object {
        internal var monthList: JSONArray
        internal var name: String
        internal var total: Double = 0.toDouble()
        private var year: Int = 0
    }


}
