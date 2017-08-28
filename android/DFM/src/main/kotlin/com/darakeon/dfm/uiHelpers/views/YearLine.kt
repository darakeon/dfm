package com.darakeon.dfm.uiHelpers.views

import android.content.Context
import android.content.Intent
import android.util.AttributeSet
import android.view.View
import android.widget.LinearLayout
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.ExtractActivity
import com.darakeon.dfm.activities.base.setColorByAttr
import com.darakeon.dfm.uiHelpers.adapters.YearAdapter
import org.json.JSONException
import java.text.DecimalFormat

class YearLine(context: Context, attributeSet: AttributeSet) : LinearLayout(context, attributeSet) {

    override fun onFinishInflate() {
        super.onFinishInflate()
    }

    val MonthField: TextView get() = findViewById(R.id.month) as TextView
    val TotalField: TextView get() = findViewById(R.id.value) as TextView

    @Throws(JSONException::class)
    fun setYear(year: YearAdapter.Year, color: Int) {
        setBackgroundColor(color)

        MonthField.text = year.MonthName

        val totalColor = if (year.Total < 0) R.attr.negative else R.attr.positive
        val totalToShow = if (year.Total < 0) -year.Total else year.Total
        val totalStr = DecimalFormat("#,##0.00").format(totalToShow)

        TotalField.setColorByAttr(totalColor)
        TotalField.text = totalStr

        isClickable = true

        setOnClickListener(onClickListener(context, year))
    }

    inner class onClickListener internal constructor(private val context: Context, private val year: YearAdapter.Year) : OnClickListener {

        override fun onClick(v: View) {
            val intent = Intent(context, ExtractActivity::class.java)

            intent.putExtra("accountUrl", year.AccountUrl)
            intent.putExtra("year", year.YearNumber)
            intent.putExtra("month", year.MonthNumber - 1)

            context.startActivity(intent)
        }
    }


}
