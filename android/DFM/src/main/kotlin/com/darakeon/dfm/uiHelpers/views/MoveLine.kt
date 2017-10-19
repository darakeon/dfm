package com.darakeon.dfm.uiHelpers.views

import android.content.Context
import android.util.AttributeSet
import android.view.View
import android.widget.LinearLayout
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.ExtractActivity
import com.darakeon.dfm.activities.base.applyGlyphicon
import com.darakeon.dfm.activities.base.setColorByAttr
import com.darakeon.dfm.uiHelpers.adapters.MoveAdapter
import java.text.DateFormat
import java.text.DecimalFormat

class MoveLine(context: Context, attributeSet: AttributeSet) : LinearLayout(context, attributeSet) {

	private var move: MoveAdapter.Move? = null

	private val nameField: TextView get() = findViewById(R.id.name)
	private val dateField: TextView? get() = findViewById(R.id.date)
	private val totalField: TextView get() = findViewById(R.id.value)
	private val checkedField: TextView get() = findViewById(R.id.check_move)

	fun setMove(activity: ExtractActivity, move: MoveAdapter.Move, color: Int, canCheck: Boolean) {
		setBackgroundColor(color)

		this.move = move

		nameField.text = move.description
		setTotalField(move)
		setCheckField(move, canCheck)
		setDateField(move)

		setOnClickListener { v ->
			activity.clickedView = v
			v.showContextMenu()
		}
	}

	private fun setTotalField(move: MoveAdapter.Move) {
		val totalColor = if (move.total < 0) R.attr.negative else R.attr.positive
		val totalToShow = if (move.total < 0) -move.total else move.total
		val totalStr = DecimalFormat("#,##0.00").format(totalToShow)

		totalField.setColorByAttr(totalColor)
		totalField.text = totalStr
	}

	private fun setCheckField(move: MoveAdapter.Move, canCheck: Boolean) {
		if (canCheck) {
			val textRes = if (move.checked) R.string.checked else R.string.unchecked
			checkedField.text = context.getString(textRes)

			val activity = context as ExtractActivity
			checkedField.applyGlyphicon(activity)

			val color = if (move.checked) R.attr.checked else R.attr.unchecked
			checkedField.setColorByAttr(color)

		} else {
			checkedField.visibility = View.GONE
		}
	}

	private fun setDateField(move: MoveAdapter.Move) {
		if (dateField != null) {
			val formatter = DateFormat.getDateInstance(DateFormat.SHORT)
			val dateInFull = formatter.format(move.date.time)
			dateField!!.text = dateInFull
		}
	}


	override fun getId(): Int = move?.id ?: 0

	val description: String
		get() = move?.description ?: ""

	val isChecked: Boolean
		get() = move?.checked ?: false


}
