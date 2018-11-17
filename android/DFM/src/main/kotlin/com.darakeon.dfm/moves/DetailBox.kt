package com.darakeon.dfm.moves

import android.content.Context
import android.view.View
import android.widget.Button
import android.widget.LinearLayout
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.moves.Move
import java.text.DecimalFormat

class DetailBox(
	context: Context,
	internal var move: Move?,
	internal var description: String?,
	internal var amount: Int,
	internal var value: Double
) : LinearLayout(context) {
	init {
		val descriptionField = TextView(context)
		descriptionField.text = description
		setWeight(descriptionField, 4f)
		addView(descriptionField)

		val amountField = TextView(context)
		amountField.text = String.format("%d", amount)
		setWeight(amountField, 1f)
		addView(amountField)

		val valueField = TextView(context)
		val formatter = DecimalFormat("0.00")
		valueField.text = formatter.format(value)
		setWeight(valueField, 2f)
		addView(valueField)

		val buttonField = Button(context)
		buttonField.setText(R.string.remove_detail)
		buttonField.setOnClickListener(this::removeDetail)

		setWeight(buttonField, 1f)
		addView(buttonField)
	}

	private fun removeDetail(button: View) {
		move?.remove(description, amount, value)

		val item = button.parent as LinearLayout
		(item.parent as LinearLayout).removeView(item)
	}

	private fun setWeight(field: TextView, weight: Float) {
		val params = LayoutParams(0, LayoutParams.WRAP_CONTENT, weight)
		field.layoutParams = params
	}
}
