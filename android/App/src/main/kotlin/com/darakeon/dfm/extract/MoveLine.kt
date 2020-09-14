package com.darakeon.dfm.extract

import android.content.Context
import android.content.res.Configuration.ORIENTATION_LANDSCAPE
import android.content.res.Configuration.ORIENTATION_PORTRAIT
import android.util.AttributeSet
import android.view.Menu
import android.view.View
import android.widget.LinearLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.lib.api.entities.extract.Move
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.lib.extensions.applyGlyphicon
import com.darakeon.dfm.lib.extensions.setColorByAttr
import com.darakeon.dfm.lib.extensions.setValueColored
import kotlinx.android.synthetic.main.move_line.view.check_move
import kotlinx.android.synthetic.main.move_line.view.date
import kotlinx.android.synthetic.main.move_line.view.name
import kotlinx.android.synthetic.main.move_line.view.value
import java.util.UUID

class MoveLine(
	context: Context,
	attributeSet: AttributeSet
) : LinearLayout(context, attributeSet) {
	var menu: Menu? = null

	private lateinit var move: Move
	private var checkNature: Nature = Nature.Out

	val guid: UUID
		get() = move.guid

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
		value.setValueColored(move.total)
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
		when (context.resources.configuration.orientation) {
			ORIENTATION_PORTRAIT -> {
				date?.text = move.date.day
					.toString().padStart(2, '0')
			}
			ORIENTATION_LANDSCAPE -> {
				date?.text = move.date.format()
			}
		}
	}

	private fun setCheckNature(move: Move) {
		checkNature =
			if (move.total < 0)
				Nature.Out
			else
				Nature.In
	}

	fun check() {
		isChecked = true
		setCheckField()
	}

	fun uncheck() {
		isChecked = false
		setCheckField()
	}

	val description: String
		get() = move.description

	var isChecked: Boolean
		get() = move.checked
		private set(value) {
			move.checked = value
		}

	val nature: Nature
		get() = checkNature
}
