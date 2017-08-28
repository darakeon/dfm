package com.dontflymoney.layout

import android.content.Context
import android.content.Intent
import android.graphics.Color
import android.util.AttributeSet
import android.view.View
import android.widget.LinearLayout
import android.widget.TextView

import com.dontflymoney.adapters.YearAdapter
import com.dontflymoney.view.ExtractActivity
import com.dontflymoney.view.R

import org.json.JSONException

import java.text.DecimalFormat

class YearLine(context: Context, attributeSet: AttributeSet) : LinearLayout(context, attributeSet) {

    override fun onFinishInflate() {
        super.onFinishInflate()
        MonthField = findViewById(R.id.month) as TextView
        TotalField = findViewById(R.id.value) as TextView
    }

    var MonthField: TextView
    var TotalField: TextView

    @Throws(JSONException::class)
    fun setYear(year: YearAdapter.Year, color: Int) {
        setBackgroundColor(color)

        MonthField.text = year.MonthName

        val totalColor = if (year.Total < 0) Color.RED else Color.BLUE
        val totalToShow = if (year.Total < 0) -year.Total else year.Total
        val totalStr = DecimalFormat("#,##0.00").format(totalToShow)

        TotalField.setTextColor(totalColor)
        TotalField.text = totalStr

        isClickable = true

        setOnClickListener(onClickListener(context, year))
    }

    inner class onClickListener internal constructor(private val context: Context, private val year: YearAdapter.Year) : View.OnClickListener {

        override fun onClick(v: View) {
            val intent = Intent(context, ExtractActivity::class.java)

            intent.putExtra("accountUrl", year.AccountUrl)
            intent.putExtra("year", year.YearNumber)
            intent.putExtra("month", year.MonthNumber - 1)

            context.startActivity(intent)
        }
    }


}
