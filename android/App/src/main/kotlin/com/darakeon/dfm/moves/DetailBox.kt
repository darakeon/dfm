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
	private var value: Double,
	private var conversion: Double?,
) : LinearLayout(context) {
	constructor(context: Context) : this(
		context,
		Move(),
		"",
		context.resources.getInteger(R.integer.amount_default),
		0.0,
		null,
	)

	init {
		inflate(context, R.layout.moves_detail, this)

		val decimalFormatter = DecimalFormat("0.00")

		val descriptionField = findViewById<TextView>(R.id.detail_description)
		descriptionField.text = description

		val amountField = findViewById<TextView>(R.id.detail_amount)
		amountField.text = String.format("%d", amount)

		val valueField = findViewById<TextView>(R.id.detail_value)
		valueField.text = decimalFormatter.format(value)

		val conversionField = findViewById<TextView>(R.id.detail_conversion)
		conversionField.text = decimalFormatter.format(conversion)

		val buttonField = findViewById<TextView>(R.id.detail_remove)
		buttonField.setOnClickListener { removeDetail() }
	}

	private fun removeDetail() : Boolean {
		move.remove(description, amount, value, conversion)
		(parent as ViewGroup).removeView(this)
		return true
	}
}
