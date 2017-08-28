package com.darakeon.dfm.uiHelpers.views

import android.content.Context
import android.util.AttributeSet
import android.view.View
import android.widget.LinearLayout
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.ExtractActivity
import com.darakeon.dfm.activities.base.setColorByAttr
import com.darakeon.dfm.uiHelpers.adapters.MoveAdapter
import org.json.JSONException
import java.text.DateFormat
import java.text.DecimalFormat

class MoveLine(context: Context, attributeSet: AttributeSet) : LinearLayout(context, attributeSet) {

    override fun onFinishInflate() {
        super.onFinishInflate()
    }

    private var move: MoveAdapter.Move? = null

    val NameField: TextView get() = findViewById(R.id.name) as TextView
    val DateField: TextView? get() = findViewById(R.id.date) as? TextView
    val TotalField: TextView get() = findViewById(R.id.value) as TextView
    val CheckedField: TextView get() = findViewById(R.id.check_move) as TextView

    @Throws(JSONException::class)
    fun setMove(activity: ExtractActivity, move: MoveAdapter.Move, color: Int, canCheck: Boolean) {
        setBackgroundColor(color)

        this.move = move

        NameField.text = move.Description
        setTotalField(move)
        setCheckField(move, canCheck)
        setDateField(move)

        setOnClickListener { v ->
            activity.clickedView = v
            v.showContextMenu()
        }
    }

    private fun setTotalField(move: MoveAdapter.Move) {
        val totalColor = if (move.Total < 0) R.attr.negative else R.attr.positive
        val totalToShow = if (move.Total < 0) -move.Total else move.Total
        val totalStr = DecimalFormat("#,##0.00").format(totalToShow)

        TotalField.setColorByAttr(totalColor)
        TotalField.text = totalStr
    }

    private fun setCheckField(move: MoveAdapter.Move, canCheck: Boolean) {
        if (canCheck) {
            val textRes = if (move.Checked) R.string.checked else R.string.unchecked
            CheckedField.text = context.getString(textRes)

            val activity = context as ExtractActivity
            CheckedField.setTypeface(activity.glyphicon)

            val color = if (move.Checked) R.attr.checked else R.attr.unchecked
            CheckedField.setColorByAttr(color)

        } else {
            CheckedField.visibility = View.GONE
        }
    }

    private fun setDateField(move: MoveAdapter.Move) {
        if (DateField != null) {
            val formatter = DateFormat.getDateInstance(DateFormat.SHORT)
            val dateInFull = formatter.format(move.Date.time)
            DateField!!.text = dateInFull
        }
    }


    override fun getId(): Int {
        return move?.ID ?: 0
    }

    val description: String
        get() = move?.Description ?: ""

    val isChecked: Boolean
        get() = move?.Checked ?: false


}
