package com.darakeon.dfm.moves

import android.graphics.Paint
import android.os.Bundle
import android.view.View
import android.view.View.FOCUS_DOWN
import android.view.View.GONE
import android.view.View.VISIBLE
import android.widget.GridLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Date
import com.darakeon.dfm.api.entities.moves.Move
import com.darakeon.dfm.api.entities.moves.MoveCreation
import com.darakeon.dfm.api.entities.moves.MoveForm
import com.darakeon.dfm.api.entities.moves.Nature
import com.darakeon.dfm.api.entities.setLabel
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
import kotlinx.android.synthetic.main.moves.account_out
import kotlinx.android.synthetic.main.moves.category
import kotlinx.android.synthetic.main.moves.date
import kotlinx.android.synthetic.main.moves.date_picker
import kotlinx.android.synthetic.main.moves.description
import kotlinx.android.synthetic.main.moves.detail_amount
import kotlinx.android.synthetic.main.moves.detail_description
import kotlinx.android.synthetic.main.moves.detail_value
import kotlinx.android.synthetic.main.moves.detailed_value
import kotlinx.android.synthetic.main.moves.details
import kotlinx.android.synthetic.main.moves.form
import kotlinx.android.synthetic.main.moves.nature
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

		date_picker.applyGlyphicon()

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
				moveForm.categoryList
					.setLabel(move.categoryName, category)
				category.setOnClickListener { changeCategory() }
			}
			move.warnCategory -> {
				category.paintFlags += Paint.STRIKE_THRU_TEXT_FLAG
				category.setOnClickListener { warnLoseCategory() }
			}
			else -> {
				category.visibility = GONE
				val layoutParams = date.layoutParams as GridLayout.LayoutParams
				layoutParams.columnSpec = GridLayout.spec(0, 5, 5f)

				val lp = category.layoutParams as GridLayout.LayoutParams
				lp.columnSpec = GridLayout.spec(0, 0, 0f)
			}
		}

		val nature = move.natureEnum
		val natureOption = moveForm.natureList.firstOrNull {
			it.value.toInt() == nature?.value
		} ?: moveForm.natureList.first()
		setNature(natureOption.text, natureOption.value.toInt())

		moveForm.accountList.setLabel(move.outUrl, account_out)
		moveForm.accountList.setLabel(move.inUrl, account_in)

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

	private fun setNature(text: String, value: Int) {
		nature.text = text
		move.natureEnum = Nature.get(value)

		val accountOutVisibility = if (move.natureEnum != Nature.In) VISIBLE else GONE
		account_out.visibility = accountOutVisibility

		val accountInVisibility = if (move.natureEnum != Nature.Out) VISIBLE else GONE
		account_in.visibility = accountInVisibility

		if (move.natureEnum == Nature.Out) {
			move.inUrl = null
			account_in.text = getString(R.string.account_in)
		}

		if (move.natureEnum == Nature.In) {
			move.outUrl = null
			account_out.text = getString(R.string.account_out)
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
		showChangeList(moveForm.categoryList, R.string.category) { text, value ->
			category.text = text
			move.categoryName = value
		}
	}

	fun warnLoseCategory() {
		this.alertError(R.string.losingCategory)
	}

	fun changeNature(@Suppress(ON_CLICK) view: View) {
		showChangeList(moveForm.natureList, R.string.nature) { text, value ->
			setNature(text, value.toInt())
		}
	}

	fun changeAccountOut(@Suppress(ON_CLICK) view: View) {
		showChangeList(moveForm.accountList, R.string.account) { text, value ->
			account_out.text = text
			move.outUrl = value
		}
	}

	fun changeAccountIn(@Suppress(ON_CLICK) view: View) {
		showChangeList(moveForm.accountList, R.string.account) { text, value ->
			account_in.text = text
			move.inUrl = value
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
