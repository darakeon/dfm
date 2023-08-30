package com.darakeon.dfm.extract

import android.content.Context
import android.content.res.Configuration.ORIENTATION_LANDSCAPE
import android.content.res.Configuration.ORIENTATION_PORTRAIT
import android.util.AttributeSet
import android.widget.LinearLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.databinding.MoveDetailsBinding
import com.darakeon.dfm.databinding.MoveLineBinding
import com.darakeon.dfm.lib.api.entities.extract.Move
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.lib.extensions.applyGlyphicon
import com.darakeon.dfm.lib.extensions.setColorByAttr
import com.darakeon.dfm.lib.extensions.setValueColored
import java.util.UUID

class MoveLine(
	context: Context,
	attributeSet: AttributeSet
) : LinearLayout(context, attributeSet) {
	private lateinit var move: Move
	private lateinit var binding: MoveLineBinding
	private lateinit var detailBinding: MoveDetailsBinding
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

		binding = MoveLineBinding.bind(this)
		detailBinding = binding.details

		detailBinding.name.text = move.description
		setTotalField(move)
		setCheckField(canCheck)
		setCheckNature(move)
		setDateField(move)

		setActions(edit, delete, check, uncheck)

		setOnLongClickListener {
			if (binding.actions.visibility == GONE)
				binding.actions.visibility = VISIBLE
			else
				binding.actions.visibility = GONE

			true
		}
	}

	private fun setTotalField(move: Move) {
		detailBinding.value.setValueColored(move.total)
	}

	private fun setCheckField(canCheck: Boolean) {
		if (canCheck) {
			setCheckField()
		} else {
			binding.actionCheck.visibility = GONE
			binding.actionUncheck.visibility = GONE
			detailBinding.moveStatus.visibility = GONE
		}
	}

	private fun setCheckField() {
		val textRes = if (isChecked) R.string.glyph_checked else R.string.glyph_unchecked
		detailBinding.moveStatus.text = context.getString(textRes)

		detailBinding.moveStatus.applyGlyphicon()

		val color = if (isChecked) R.attr.checked else R.attr.unchecked
		detailBinding.moveStatus.setColorByAttr(color)

		if (isChecked) {
			binding.actionCheck.visibility = GONE
			binding.actionUncheck.visibility = VISIBLE
		}
		else {
			binding.actionCheck.visibility = VISIBLE
			binding.actionUncheck.visibility = GONE
		}
	}

	private fun setDateField(move: Move) {
		when (context.resources.configuration.orientation) {
			ORIENTATION_PORTRAIT -> {
				detailBinding.date.text = move.date.day
					.toString().padStart(2, '0')
			}
			ORIENTATION_LANDSCAPE -> {
				detailBinding.date.text = move.date.format()
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
		binding.actionEdit.applyGlyphicon()
		binding.actionEdit.setOnClickListener { edit(this) }

		binding.actionDelete.applyGlyphicon()
		binding.actionDelete.setOnClickListener { delete(this) }

		binding.actionCheck.applyGlyphicon()
		binding.actionCheck.setOnClickListener { check(this) }

		binding.actionUncheck.applyGlyphicon()
		binding.actionUncheck.setOnClickListener { uncheck(this) }
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
