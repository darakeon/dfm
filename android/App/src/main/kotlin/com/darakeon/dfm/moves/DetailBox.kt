package com.darakeon.dfm.moves

import android.content.Context
import android.view.ViewGroup
import android.widget.LinearLayout
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.lib.api.entities.moves.Move
import java.text.DecimalFormat

class DetailBox(
	context: Context,
	private var move: Move,
	private var description: String,
	private var amount: Int,
	private var value: Double
) : LinearLayout(context) {
	constructor(context: Context) : this(
		context,
		Move(),
		"",
		context.resources.getInteger(R.integer.amount_default),
		0.0
	)

	init {
		inflate(context, R.layout.moves_detail, this)

		val descriptionField = findViewById<TextView>(R.id.detail_description)
		descriptionField.text = description

		val amountField = findViewById<TextView>(R.id.detail_amount)
		amountField.text = String.format("%d", amount)

		val valueField = findViewById<TextView>(R.id.detail_value)
		val formatter = DecimalFormat("0.00")
		valueField.text = formatter.format(value)

		val buttonField = findViewById<TextView>(R.id.detail_remove)
		buttonField.setOnClickListener { removeDetail() }
	}

	private fun removeDetail() : Boolean {
		move.remove(description, amount, value)
		(parent as ViewGroup).removeView(this)
		return true
	}
}
