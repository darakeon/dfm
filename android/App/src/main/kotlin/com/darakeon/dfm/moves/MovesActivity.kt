package com.darakeon.dfm.moves

import android.graphics.Paint
import android.os.Bundle
import android.view.View
import android.view.View.FOCUS_DOWN
import android.view.View.GONE
import android.view.View.VISIBLE
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.dialogs.alertError
import com.darakeon.dfm.dialogs.getDateDialog
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.addMask
import com.darakeon.dfm.extensions.backWithExtras
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.Date
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.lib.api.entities.moves.MoveCreation
import com.darakeon.dfm.lib.api.entities.moves.Nature
import com.darakeon.dfm.lib.api.entities.setCombo
import com.darakeon.dfm.lib.extensions.applyGlyphicon
import com.darakeon.dfm.lib.extensions.changeColSpan
import com.darakeon.dfm.lib.extensions.onChange
import com.darakeon.dfm.lib.extensions.showChangeList
import com.darakeon.dfm.lib.extensions.toDoubleByCulture
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
import kotlinx.android.synthetic.main.moves.main
import kotlinx.android.synthetic.main.moves.nature_in
import kotlinx.android.synthetic.main.moves.nature_out
import kotlinx.android.synthetic.main.moves.nature_transfer
import kotlinx.android.synthetic.main.moves.no_accounts
import kotlinx.android.synthetic.main.moves.no_categories
import kotlinx.android.synthetic.main.moves.remove_check
import kotlinx.android.synthetic.main.moves.simple_value
import kotlinx.android.synthetic.main.moves.value
import kotlinx.android.synthetic.main.moves.warnings
import java.util.UUID

class MovesActivity : BaseActivity() {
	override val contentView = R.layout.moves
	override val title = R.string.title_activity_move

	private var move = Move()
	private val moveKey = "move"
	private var moveForm = MoveCreation()
	private val moveFormKey = "moveForm"
	private var offlineHash = 0
	private val offlineHashKey = "offlineHash"

	private var accountUrl = ""
	private var id: UUID? = null

	private val amountDefault
		get() = resources.getInteger(
			R.integer.amount_default
		).toString()

	override val refresh: SwipeRefreshLayout?
		get() = main

	private var loadedScreen = false
	private val fallbackManager = MovesService.manager(this)

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		arrayOf(
			date_picker,
			category_picker,
			account_out_picker,
			account_in_picker
		).forEach { it.applyGlyphicon() }

		accountUrl = getExtraOrUrl("accountUrl") ?: ""
		val id = getExtraOrUrl("id")

		if (id != null)
			this.id = tryGetGuid(id)

		detail_amount.setText(amountDefault)

		if (savedInstanceState == null) {
			if (this.id != null) {
				callApi { it.getMove(this.id, this::populateScreen) }
			} else {
				move.date = Date()
				populateResponse()
			}
		} else {
			move = savedInstanceState.getFromJson(moveKey, Move())
			moveForm = savedInstanceState.getFromJson(moveFormKey, MoveCreation())
			offlineHash = savedInstanceState.getInt(offlineHashKey, 0)

			populateResponse()
		}
	}

	private fun tryGetGuid(id: String?) =
		try {
			UUID.fromString(id)
		} catch (e: IllegalArgumentException) {
			null
		}

	private fun populateScreen(data: MoveCreation) {
		moveForm = data

		val move = data.move
		var error = ""

		if (move != null) {
			this.move = move
		} else {
			fallbackManager.printCounting()
			val previousError = fallbackManager.error

			if (previousError == null) {
				this.move.date = Date()
			} else {
				this.move = previousError.obj
				error = previousError.error
				offlineHash = previousError.hashCode()
			}
		}

		populateResponse()

		if (error != "") {
			val tried = getString(R.string.background_move_error)
			alertError("$tried: $error")
		}
	}

	private fun populateResponse() {
		move.setDefaultData(accountUrl, isUsingCategories)

		if (!isMoveAllowed())
			return

		main.scrollChild = form

		if (move.checked)
			remove_check.visibility = VISIBLE

		description.setText(move.description)
		description.onChange { move.description = it }

		populateDate()
		populateCategory()

		populateAccounts()
		setNatureFromAccounts()
		populateValue()

		loadedScreen = true
	}

	private fun isMoveAllowed(): Boolean {
		var canMove = true

		if (blockedByAccounts()) {
			canMove = false
			no_accounts.visibility = VISIBLE
		}

		if (blockedByCategories()) {
			canMove = false
			no_categories.visibility = VISIBLE
		}

		if (!canMove) {
			form.visibility = GONE
			warnings.visibility = VISIBLE
		}

		return canMove
	}

	private fun blockedByAccounts() =
		accountCombo.isEmpty()

	private fun blockedByCategories() =
		isUsingCategories && categoryCombo.isEmpty()

	private fun populateDate() {
		date.addMask("####-##-##")
		date.setText(move.date.format())
		date.onChange { move.date = Date(it) }
		date_picker.setOnClickListener { showDatePicker() }
	}

	fun showDatePicker() {
		with(move.date) {
			getDateDialog(year, javaMonth, day) { y, m, d ->
				date.setText(Date(y, m+1, d).format())
			}.show()
		}
	}

	private fun populateCategory() {
		when {
			isUsingCategories -> {
				populateCategoryCombo()
			}
			move.warnCategory -> {
				setupWarnLoseCategory()
			}
			else -> {
				hideCategory()
			}
		}
	}

	private fun populateCategoryCombo() {
		categoryCombo.setCombo(
			category,
			category_picker,
			move::categoryName,
			this::changeCategory
		)
	}

	fun changeCategory() {
		showChangeList(categoryCombo, R.string.category) {
			text, _ -> category.setText(text)
		}
	}

	private fun setupWarnLoseCategory() {
		category.visibility = GONE
		category.changeColSpan(0)
		category_picker.changeColSpan(4)

		category_picker.text = move.categoryName
		category_picker.paintFlags += Paint.STRIKE_THRU_TEXT_FLAG
		category_picker.setOnClickListener {
			this.alertError(R.string.losingCategory)
		}
	}

	private fun hideCategory() {
		category.visibility = GONE
		category_picker.visibility = GONE

		date.changeColSpan(7)
		category.changeColSpan(0)
		category_picker.changeColSpan(0)
	}

	private fun populateAccounts() {
		accountCombo.setCombo(
			account_out,
			account_out_picker,
			move::outUrl,
			this::changeAccountOut
		)
		account_out.onChange {
			setNatureFromAccounts()
		}

		accountCombo.setCombo(
			account_in,
			account_in_picker,
			move::inUrl,
			this::changeAccountIn
		)
		account_in.onChange {
			setNatureFromAccounts()
		}
	}

	fun changeAccountOut() {
		showChangeList(accountCombo, R.string.account) { text, _ ->
			account_out.setText(text)
		}
	}

	fun changeAccountIn() {
		showChangeList(accountCombo, R.string.account) { text, _ ->
			account_in.setText(text)
		}
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

		runOnUiThread {
			nature_out.isChecked = false
			nature_transfer.isChecked = false
			nature_in.isChecked = false

			when (move.natureEnum) {
				Nature.Out -> nature_out.isChecked = true
				Nature.Transfer -> nature_transfer.isChecked = true
				Nature.In -> nature_in.isChecked = true
				else -> {}
			}
		}
	}

	private fun populateValue() {
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

	private fun useDetailed() {
		move.isDetailed = true

		simple_value.visibility = GONE
		detailed_value.visibility = VISIBLE

		account_in.nextFocusDownId = R.id.detail_description

		scrollToTheEnd(detail_description)
	}

	private fun addViewDetail(move: Move, description: String, amount: Int, value: Double) {
		val row = DetailBox(this, move, description, amount, value)
		details.addView(row)
	}

	private fun useSimple() {
		move.isDetailed = false

		simple_value.visibility = VISIBLE
		detailed_value.visibility = GONE

		account_in.nextFocusDownId = R.id.value

		scrollToTheEnd(value)
	}

	private fun scrollToTheEnd(view: View) {
		form.post {
			form.fullScroll(FOCUS_DOWN)
			view.requestFocus()
		}
	}

	override fun onSaveInstanceState(outState: Bundle) {
		super.onSaveInstanceState(outState)
		outState.putJson(moveKey, move)
		outState.putJson(moveFormKey, moveForm)
		outState.putInt(offlineHashKey, offlineHash)
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

		detail_description.setText("")
		detail_amount.setText(amountDefault)
		detail_value.setText("")

		move.add(description, amount, value)

		addViewDetail(move, description, amount, value)

		scrollToTheEnd(detail_description)
	}

	fun save(@Suppress(ON_CLICK) view: View) {
		move.clearNotUsedValues()

		callApi {
			it.saveMove(move) {
				fallbackManager.remove(offlineHash)
				clearAndBack()
			}
		}
	}

	override fun offline() {
		if (loadedScreen) {
			error(R.string.offline_fallback_moves, R.string.ok_button) {
				MovesService.start(this, move)
				clearAndBack()
			}
		} else {
			super.offline()
		}
	}

	private fun clearAndBack() {
		intent.removeExtra("id")
		backWithExtras()
	}

	fun cancel(@Suppress(ON_CLICK) view: View) {
		fallbackManager.remove(offlineHash)
		backWithExtras()
	}
}
