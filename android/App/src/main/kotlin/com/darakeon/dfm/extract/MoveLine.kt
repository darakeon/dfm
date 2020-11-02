package com.darakeon.dfm.extract

import android.content.Context
import android.content.res.Configuration.ORIENTATION_LANDSCAPE
import android.content.res.Configuration.ORIENTATION_PORTRAIT
import android.util.AttributeSet
import android.widget.LinearLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.lib.api.entities.extract.Move
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.lib.extensions.applyGlyphicon
import com.darakeon.dfm.lib.extensions.setColorByAttr
import com.darakeon.dfm.lib.extensions.setValueColored
import kotlinx.android.synthetic.main.move_details.view.date
import kotlinx.android.synthetic.main.move_details.view.move_status
import kotlinx.android.synthetic.main.move_details.view.name
import kotlinx.android.synthetic.main.move_details.view.value
import kotlinx.android.synthetic.main.move_line.view.action_check
import kotlinx.android.synthetic.main.move_line.view.action_delete
import kotlinx.android.synthetic.main.move_line.view.action_edit
import kotlinx.android.synthetic.main.move_line.view.action_uncheck
import kotlinx.android.synthetic.main.move_line.view.actions
import java.util.UUID

class MoveLine(
	context: Context,
	attributeSet: AttributeSet
) : LinearLayout(context, attributeSet) {
	private lateinit var move: Move
	private var checkNature: Nature = Nature.Out

	val guid: UUID
		get() = move.guid

	fun setMove(
		move: Move,
		canCheck: Boolean,
		edit: (MoveLine) -> Unit,
		delete: (MoveLine) -> Unit,
		check: (MoveLine) -> Unit,
		uncheck: (MoveLine) -> Unit,
	) {
		this.move = move

		name.text = move.description
		setTotalField(move)
		setCheckField(canCheck)
		setCheckNature(move)
		setDateField(move)

		setActions(edit, delete, check, uncheck)

		setOnLongClickListener {
			if (actions.visibility == GONE)
				actions.visibility = VISIBLE
			else
				actions.visibility = GONE

			true
		}
	}

	private fun setTotalField(move: Move) {
		value.setValueColored(move.total)
	}

	private fun setCheckField(canCheck: Boolean) {
		if (canCheck) {
			setCheckField()
		} else {
			action_check.visibility = GONE
			action_uncheck.visibility = GONE
			move_status.visibility = GONE
		}
	}

	private fun setCheckField() {
		val textRes = if (isChecked) R.string.glyph_checked else R.string.glyph_unchecked
		move_status.text = context.getString(textRes)

		move_status.applyGlyphicon()

		val color = if (isChecked) R.attr.checked else R.attr.unchecked
		move_status.setColorByAttr(color)

		if (isChecked) {
			action_check.visibility = GONE
			action_uncheck.visibility = VISIBLE
		}
		else {
			action_check.visibility = VISIBLE
			action_uncheck.visibility = GONE
		}
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

	private fun setActions(
		edit: (MoveLine) -> Unit,
		delete: (MoveLine) -> Unit,
		check: (MoveLine) -> Unit,
		uncheck: (MoveLine) -> Unit,
	) {
		action_edit.applyGlyphicon()
		action_edit.setOnClickListener { edit(this) }

		action_delete.applyGlyphicon()
		action_delete.setOnClickListener { delete(this) }

		action_check.applyGlyphicon()
		action_check.setOnClickListener { check(this) }

		action_uncheck.applyGlyphicon()
		action_uncheck.setOnClickListener { uncheck(this) }
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
