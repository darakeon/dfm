package com.dontflymoney.layout

import android.content.Context
import android.graphics.Color
import android.util.AttributeSet
import android.view.View
import android.widget.ImageView
import android.widget.LinearLayout
import android.widget.TextView
import com.dontflymoney.adapters.MoveAdapter
import com.dontflymoney.view.ExtractActivity
import com.dontflymoney.view.R
import org.json.JSONException
import java.text.DateFormat
import java.text.DecimalFormat

class MoveLine(context: Context, attributeSet: AttributeSet) : LinearLayout(context, attributeSet) {

    override fun onFinishInflate() {
        super.onFinishInflate()
    }

    private var move: MoveAdapter.Move? = null

    val NameField: TextView get() = findViewById(R.id.name) as TextView
    val DateField: TextView get() = findViewById(R.id.date) as TextView
    val TotalField: TextView get() = findViewById(R.id.value) as TextView
    val CheckedField: ImageView get() = findViewById(R.id.check_move) as ImageView

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
        val totalColor = if (move.Total < 0) Color.RED else Color.BLUE
        val totalToShow = if (move.Total < 0) -move.Total else move.Total
        val totalStr = DecimalFormat("#,##0.00").format(totalToShow)

        TotalField.setTextColor(totalColor)
        TotalField.text = totalStr
    }

    private fun setCheckField(move: MoveAdapter.Move, canCheck: Boolean) {
        if (canCheck) {
            val idResource = if (move.Checked) R.drawable.green_sign else R.drawable.red_sign
            CheckedField.setImageResource(idResource)
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
        return move!!.ID
    }

    val description: String
        get() = move!!.Description

    val isChecked: Boolean
        get() = move!!.Checked!!


}
