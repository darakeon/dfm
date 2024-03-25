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
import com.darakeon.dfm.databinding.BottomMenuBinding
import com.darakeon.dfm.databinding.MovesBinding
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
import java.util.UUID

class MovesActivity : BaseActivity<MovesBinding>() {
	override fun inflateBinding(): MovesBinding {
		return MovesBinding.inflate(layoutInflater)
	}
	override fun getMenuBinding(): BottomMenuBinding {
		return binding.bottomMenu
	}

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

	override val refresh: SwipeRefreshLayout
		get() = binding.main

	private var loadedScreen = false
	private val fallbackManager = MovesService.manager(this)

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		arrayOf(
			binding.datePicker,
			binding.categoryPicker,
			binding.accountOutPicker,
			binding.accountInPicker
		).forEach { it.applyGlyphicon() }

		accountUrl = getExtraOrUrl("accountUrl") ?: ""
		val extraId = getExtraOrUrl("id")

		if (extraId != null)
			this.id = tryGetGuid(extraId)

		binding.detailAmount.setText(amountDefault)

		if (savedInstanceState == null) {
			val id = this.id
			if (id != null) {
				callApi { it.getMove(id, this::populateScreen) }
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

		binding.main.scrollChild = binding.form

		if (move.checked)
			binding.removeCheck.visibility = VISIBLE

		binding.description.setText(move.description)
		binding.description.onChange { move.description = it }

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
			binding.noAccounts.visibility = VISIBLE
		}

		if (blockedByCategories()) {
			canMove = false
			binding.noCategories.visibility = VISIBLE
		}

		if (!canMove) {
			binding.form.visibility = GONE
			binding.warnings.visibility = VISIBLE
		}

		return canMove
	}

	private fun blockedByAccounts() =
		accountCombo.isEmpty()

	private fun blockedByCategories() =
		isUsingCategories && categoryCombo.isEmpty()

	private fun populateDate() {
		binding.date.addMask("####-##-##")
		binding.date.setText(move.date.format())
		binding.date.onChange { move.date = Date(it) }
		binding.datePicker.setOnClickListener { showDatePicker() }
	}

	fun showDatePicker() {
		with(move.date) {
			getDateDialog(year, javaMonth, day) { y, m, d ->
				binding.date.setText(Date(y, m+1, d).format())
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
			binding.category,
			binding.categoryPicker,
			move::categoryName,
			this::changeCategory
		)
	}

	fun changeCategory() {
		showChangeList(categoryCombo, R.string.category) {
			text, _ -> binding.category.setText(text)
		}
	}

	private fun setupWarnLoseCategory() {
		binding.category.visibility = GONE
		binding.category.changeColSpan(0)
		binding.categoryPicker.changeColSpan(4)

		binding.categoryPicker.text = move.categoryName
		binding.categoryPicker.paintFlags += Paint.STRIKE_THRU_TEXT_FLAG
		binding.categoryPicker.setOnClickListener {
			this.alertError(R.string.losingCategory)
		}
	}

	private fun hideCategory() {
		binding.category.visibility = GONE
		binding.categoryPicker.visibility = GONE

		binding.date.changeColSpan(7)
		binding.category.changeColSpan(0)
		binding.categoryPicker.changeColSpan(0)
	}

	private fun populateAccounts() {
		accountCombo.setCombo(
			binding.accountOut,
			binding.accountOutPicker,
			move::outUrl,
			this::changeAccountOut
		)
		binding.accountOut.onChange {
			setNatureFromAccounts()
		}

		accountCombo.setCombo(
			binding.accountIn,
			binding.accountInPicker,
			move::inUrl,
			this::changeAccountIn
		)
		binding.accountIn.onChange {
			setNatureFromAccounts()
		}
	}

	fun changeAccountOut() {
		showChangeList(accountCombo, R.string.account) { text, _ ->
			binding.accountOut.setText(text)
		}
	}

	fun changeAccountIn() {
		showChangeList(accountCombo, R.string.account) { text, _ ->
			binding.accountIn.setText(text)
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
			binding.natureOut.isChecked = false
			binding.natureTransfer.isChecked = false
			binding.natureIn.isChecked = false

			when (move.natureEnum) {
				Nature.Out -> binding.natureOut.isChecked = true
				Nature.Transfer -> binding.natureTransfer.isChecked = true
				Nature.In -> binding.natureIn.isChecked = true
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
			binding.value.setText(String.format("%1$,.2f", move.value))
		}

		binding.value.onChange { move.setValue(it) }
	}

	private fun useDetailed() {
		move.isDetailed = true

		binding.simpleValue.visibility = GONE
		binding.detailedValue.visibility = VISIBLE

		binding.accountIn.nextFocusDownId = R.id.detail_description

		scrollToTheEnd(binding.detailDescription)
	}

	private fun addViewDetail(move: Move, description: String, amount: Int, value: Double) {
		val row = DetailBox(this, move, description, amount, value)
		binding.details.addView(row)
	}

	private fun useSimple() {
		move.isDetailed = false

		binding.simpleValue.visibility = VISIBLE
		binding.detailedValue.visibility = GONE

		binding.accountIn.nextFocusDownId = R.id.value

		scrollToTheEnd(binding.value)
	}

	private fun scrollToTheEnd(view: View) {
		binding.form.post {
			binding.form.fullScroll(FOCUS_DOWN)
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
		val description = binding.detailDescription.text.toString()
		val amount = binding.detailAmount.text.toString().toIntOrNull()
		val value = binding.detailValue.text.toString().toDoubleByCulture()

		if (description.isEmpty() || amount == null || value == null) {
			alertError(R.string.fill_all)
			return
		}

		binding.detailDescription.setText("")
		binding.detailAmount.setText(amountDefault)
		binding.detailValue.setText("")

		move.add(description, amount, value)

		addViewDetail(move, description, amount, value)

		scrollToTheEnd(binding.detailDescription)
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
