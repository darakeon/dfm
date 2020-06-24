package com.darakeon.dfm.moves

import android.graphics.Paint
import android.os.Bundle
import android.view.View
import android.view.View.FOCUS_DOWN
import android.view.View.GONE
import android.view.View.VISIBLE
import android.widget.GridLayout
import android.widget.GridLayout.UNDEFINED
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Date
import com.darakeon.dfm.api.entities.moves.Move
import com.darakeon.dfm.api.entities.moves.MoveCreation
import com.darakeon.dfm.api.entities.moves.MoveForm
import com.darakeon.dfm.api.entities.moves.Nature
import com.darakeon.dfm.api.entities.setCombo
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.dialogs.alertError
import com.darakeon.dfm.dialogs.getDateDialog
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.addMask
import com.darakeon.dfm.extensions.applyGlyphicon
import com.darakeon.dfm.extensions.backWithExtras
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.onChange
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.extensions.showChangeList
import com.darakeon.dfm.extensions.toDoubleByCulture
import kotlinx.android.synthetic.main.moves.account_in
import kotlinx.android.synthetic.main.moves.account_in_picker
import kotlinx.android.synthetic.main.moves.account_out
import kotlinx.android.synthetic.main.moves.account_out_picker
import kotlinx.android.synthetic.main.moves.category
import kotlinx.android.synthetic.main.moves.category_picker
import kotlinx.android.synthetic.main.moves.date
import kotlinx.android.synthetic.main.moves.date_picker
import kotlinx.android.synthetic.main.moves.description
import kotlinx.android.synthetic.main.moves.detail_amount
import kotlinx.android.synthetic.main.moves.detail_description
import kotlinx.android.synthetic.main.moves.detail_value
import kotlinx.android.synthetic.main.moves.detailed_value
import kotlinx.android.synthetic.main.moves.details
import kotlinx.android.synthetic.main.moves.form
import kotlinx.android.synthetic.main.moves.nature_in
import kotlinx.android.synthetic.main.moves.nature_out
import kotlinx.android.synthetic.main.moves.nature_transfer
import kotlinx.android.synthetic.main.moves.no_accounts
import kotlinx.android.synthetic.main.moves.no_categories
import kotlinx.android.synthetic.main.moves.remove_check
import kotlinx.android.synthetic.main.moves.simple_value
import kotlinx.android.synthetic.main.moves.value
import kotlinx.android.synthetic.main.moves.warnings

class MovesActivity : BaseActivity() {
	override val contentView = R.layout.moves
	override val title = R.string.title_activity_move

	private var move = Move()
	private val moveKey = "move"
	private var moveForm: MoveForm = MoveCreation()
	private val moveFormKey = "moveForm"

	private var accountUrl = ""
	private var id = 0

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		arrayOf(
			date_picker,
			category_picker,
			account_out_picker,
			account_in_picker
		).forEach { it.applyGlyphicon() }

		accountUrl = getExtraOrUrl("accountUrl") ?: ""
		id = getExtraOrUrl("id")?.toIntOrNull() ?: 0

		if (savedInstanceState == null) {
			callApi { it.getMove(id, this::populateScreen) }
		} else {
			move = savedInstanceState.getFromJson(moveKey, Move())
			moveForm = savedInstanceState.getFromJson(moveFormKey, MoveCreation())

			populateResponse()
		}
	}

	private fun populateScreen(data: MoveCreation) {
		moveForm = data

		if (data.move != null)
			move = data.move
		else
			move.date = Date()

		populateResponse()
	}

	private fun populateResponse() {
		move.setDefaultData(accountUrl, moveForm.isUsingCategories)

		if (!isMoveAllowed())
			return

		if (move.checked)
			remove_check.visibility = VISIBLE

		description.setText(move.description)
		description.onChange { move.description = it }

		date.addMask("####-##-##")
		date.setText(move.date.format())
		date.onChange { move.date = Date(it) }
		date_picker.setOnClickListener { showDatePicker() }

		when {
			moveForm.isUsingCategories -> {
				moveForm.categoryList.setCombo(
					category,
					category_picker,
					move::categoryName,
					this::changeCategory
				)
			}
			move.warnCategory -> {
				category.visibility = GONE
				changeColSpan(category, 0)
				changeColSpan(category_picker, 4)

				category_picker.text = move.categoryName
				category_picker.paintFlags += Paint.STRIKE_THRU_TEXT_FLAG
				category_picker.setOnClickListener { warnLoseCategory() }
			}
			else -> {
				category.visibility = GONE
				category_picker.visibility = GONE

				changeColSpan(date, 7)
				changeColSpan(category, 0)
				changeColSpan(category_picker, 0)
			}
		}

		setNatureFromEnum()

		moveForm.accountList.setCombo(
			account_out,
			account_out_picker,
			move::outUrl,
			this::changeAccountOut
		)
		account_out.onChange { setNatureFromAccounts() }

		moveForm.accountList.setCombo(
			account_in,
			account_in_picker,
			move::inUrl,
			this::changeAccountIn
		)
		account_in.onChange { setNatureFromAccounts() }

		if (move.detailList.isNotEmpty()) {
			useDetailed()
			move.detailList.forEach {
				addViewDetail(move, it.description, it.amount, it.value)
			}
		} else if (move.value != null) {
			useSimple()
			value.setText(String.format("%1$,.2f", move.value))
		}

		value.onChange { move.setValue(it) }
	}

	private fun setNatureFromAccounts() {
		val hasOut = !move.outUrl.isNullOrBlank()
		val hasIn = !move.inUrl.isNullOrBlank()

		when {
			hasOut && hasIn -> move.natureEnum = Nature.Transfer
			hasOut -> move.natureEnum = Nature.Out
			hasIn -> move.natureEnum = Nature.In
			else -> move.natureEnum = null
		}

		setNatureFromEnum()
	}

	private fun changeColSpan(view: View, size: Int) {
		val layoutParams = view.layoutParams as GridLayout.LayoutParams
		layoutParams.columnSpec = GridLayout.spec(UNDEFINED, size, size.toFloat())
	}

	private fun isMoveAllowed(): Boolean {
		var canMove = true

		if (moveForm.blockedByAccounts()) {
			canMove = false
			no_accounts.visibility = VISIBLE
		}

		if (moveForm.blockedByCategories()) {
			canMove = false
			no_categories.visibility = VISIBLE
		}

		if (!canMove) {
			form.visibility = GONE
			warnings.visibility = VISIBLE
		}

		return canMove
	}

	private fun setNatureFromEnum() {
		nature_out.isChecked = false
		nature_transfer.isChecked = false
		nature_in.isChecked = false

		when (move.natureEnum) {
			Nature.Out -> nature_out.isChecked = true
			Nature.Transfer -> nature_transfer.isChecked = true
			Nature.In -> nature_in.isChecked = true
		}
	}

	private fun useDetailed() {
		move.isDetailed = true

		simple_value.visibility = GONE
		detailed_value.visibility = VISIBLE

		scrollToTheEnd()
	}

	private fun useSimple() {
		move.isDetailed = false

		simple_value.visibility = VISIBLE
		detailed_value.visibility = GONE

		scrollToTheEnd()
	}

	private fun scrollToTheEnd() {
		form.postDelayed({
			form.fullScroll(FOCUS_DOWN)
		}, 100)
	}

	private fun addViewDetail(move: Move, description: String, amount: Int, value: Double) {
		val row = DetailBox(this, move, description, amount, value)
		details.addView(row)
	}

	override fun onSaveInstanceState(outState: Bundle) {
		super.onSaveInstanceState(outState)
		outState.putJson(moveKey, move)
		outState.putJson(moveFormKey, moveForm)
	}

	fun showDatePicker() {
		with(move.date) {
			getDateDialog(year, javaMonth, day) { y, m, d ->
				date.setText(Date(y, m+1, d).format())
			}.show()
		}
	}

	fun changeCategory() {
		showChangeList(moveForm.categoryList, R.string.category) {
			text, _ -> category.setText(text)
		}
	}

	fun warnLoseCategory() {
		this.alertError(R.string.losingCategory)
	}

	fun changeAccountOut() {
		showChangeList(moveForm.accountList, R.string.account) { text, _ ->
			account_out.setText(text)
		}
	}

	fun changeAccountIn() {
		showChangeList(moveForm.accountList, R.string.account) { text, _ ->
			account_in.setText(text)
		}
	}

	fun useDetailed(@Suppress(ON_CLICK) view: View) {
		useDetailed()
	}

	fun useSimple(@Suppress(ON_CLICK) view: View) {
		useSimple()
	}

	fun addDetail(@Suppress(ON_CLICK) view: View) {
		val description = detail_description.text.toString()
		val amount = detail_amount.text.toString().toIntOrNull()
		val value = detail_value.text.toString().toDoubleByCulture()

		if (description.isEmpty() || amount == null || value == null) {
			alertError(R.string.fill_all)
			return
		}

		val amountDefault = resources.getInteger(R.integer.amount_default)

		detail_description.setText("")
		detail_amount.setText(amountDefault.toString())
		detail_value.setText("")

		move.add(description, amount, value)

		addViewDetail(move, description, amount, value)

		scrollToTheEnd()
	}

	fun save(@Suppress(ON_CLICK) view: View) {
		move.clearNotUsedValues()
		callApi {
			it.saveMove(move) {
				intent.removeExtra("id")
				backWithExtras()
			}
		}
	}
}
