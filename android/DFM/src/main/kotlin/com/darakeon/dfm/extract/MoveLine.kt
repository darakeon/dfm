package com.darakeon.dfm.extract

import android.content.Context
import android.util.AttributeSet
import android.view.Menu
import android.view.View
import android.widget.LinearLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.extract.Move
import com.darakeon.dfm.api.entities.moves.Nature
import com.darakeon.dfm.extensions.applyGlyphicon
import com.darakeon.dfm.extensions.setColorByAttr
import kotlinx.android.synthetic.main.move_line.view.check_move
import kotlinx.android.synthetic.main.move_line.view.date
import kotlinx.android.synthetic.main.move_line.view.name
import kotlinx.android.synthetic.main.move_line.view.value
import java.text.DecimalFormat

class MoveLine(context: Context, attributeSet: AttributeSet) : LinearLayout(context, attributeSet) {

	var menu: Menu? = null

	private var move: Move? = null
	private var checkNature: Nature = Nature.Out

	fun setMove(move: Move, canCheck: Boolean) {
		this.move = move

		name.text = move.description
		setTotalField(move)
		setCheckField(canCheck)
		setCheckNature(move)
		setDateField(move)

		setOnClickListener {
			it.showContextMenu()
		}
	}

	private fun setTotalField(move: Move) {
		val totalColor = if (move.total < 0) R.attr.negative else R.attr.positive
		val totalToShow = if (move.total < 0) -move.total else move.total
		val totalStr = DecimalFormat("#,##0.00").format(totalToShow)

		value.setColorByAttr(totalColor)
		value.text = totalStr
	}

	private fun setCheckField(canCheck: Boolean) {
		if (canCheck) {
			setCheckField()
		} else {
			check_move.visibility = View.GONE
		}
	}

	private fun setCheckField() {
		val textRes = if (isChecked) R.string.checked else R.string.unchecked
		check_move.text = context.getString(textRes)

		check_move.applyGlyphicon()

		val color = if (isChecked) R.attr.checked else R.attr.unchecked
		check_move.setColorByAttr(color)
	}

	private fun setDateField(move: Move) {
		if (date != null) {
			date?.text = move.date.format()
		}
	}

	private fun setCheckNature(move: Move) {
		checkNature =
			if (move.total < 0)
				Nature.Out
			else
				Nature.In
	}

	override fun getId(): Int = move?.id ?: 0

	fun check() {
		isChecked = true
		setCheckField()
	}

	fun uncheck() {
		isChecked = false
		setCheckField()
	}

	val description: String
		get() = move?.description ?: ""

	var isChecked: Boolean
		get() = move?.checked ?: false
		private set(value) {
			move?.checked = value
		}

	val nature: Nature
		get() = checkNature
}
