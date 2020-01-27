package com.darakeon.dfm.moves

import android.app.DatePickerDialog
import android.graphics.Paint
import android.os.Bundle
import android.view.View
import android.widget.ScrollView
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.ComboItem
import com.darakeon.dfm.api.entities.Date
import com.darakeon.dfm.api.entities.moves.Move
import com.darakeon.dfm.api.entities.moves.MoveCreation
import com.darakeon.dfm.api.entities.moves.Nature
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.dialogs.alertError
import com.darakeon.dfm.dialogs.getDateDialog
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.backWithExtras
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.onChange
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.extensions.showChangeList
import com.darakeon.dfm.extensions.toDoubleByCulture
import kotlinx.android.synthetic.main.moves_create.account_in
import kotlinx.android.synthetic.main.moves_create.account_out
import kotlinx.android.synthetic.main.moves_create.category
import kotlinx.android.synthetic.main.moves_create.date
import kotlinx.android.synthetic.main.moves_create.description
import kotlinx.android.synthetic.main.moves_create.detail_amount
import kotlinx.android.synthetic.main.moves_create.detail_description
import kotlinx.android.synthetic.main.moves_create.detail_value
import kotlinx.android.synthetic.main.moves_create.detailed_value
import kotlinx.android.synthetic.main.moves_create.details
import kotlinx.android.synthetic.main.moves_create.form
import kotlinx.android.synthetic.main.moves_create.lose_category
import kotlinx.android.synthetic.main.moves_create.nature
import kotlinx.android.synthetic.main.moves_create.no_accounts
import kotlinx.android.synthetic.main.moves_create.no_categories
import kotlinx.android.synthetic.main.moves_create.remove_check
import kotlinx.android.synthetic.main.moves_create.simple_value
import kotlinx.android.synthetic.main.moves_create.value
import kotlinx.android.synthetic.main.moves_create.warnings

class MovesCreateActivity : BaseActivity() {
	private val dialog: DatePickerDialog
		get() = with(move.date) {
			return getDateDialog(
				{ y, m, d -> updateFromDateCombo(y, m+1, d) },
				year, javaMonth, day
			)
		}

	override val contentView = R.layout.moves_create
	override val title = R.string.title_activity_move

	private var move = Move()
	private val moveKey = "move"
	private var moveCreation = MoveCreation()
	private val moveCreationKey = "moveCreation"

	private fun updateFromDateCombo(year: Int, month: Int, day: Int) {
		move.year = year
		move.month = month
		move.day = day
		date.text = move.date.format()
	}

	private val accountUrl get() = getExtraOrUrl("accountUrl", null)
	private val id get() = getExtraOrUrl("id", "0").toInt()

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (savedInstanceState == null) {
			initScreen()
		} else {
			startScreenFromSaved(savedInstanceState)
		}
	}

	override fun onSaveInstanceState(outState: Bundle) {
		super.onSaveInstanceState(outState)
		outState.putJson(moveKey, move)
		outState.putJson(moveCreationKey, moveCreation)
	}

	private fun initScreen() {
		api.getMove(id, this::populateScreen)

		setCurrentDate()
		setControls()
	}

	private fun startScreenFromSaved(savedInstanceState: Bundle) {
		moveCreation = savedInstanceState.getFromJson(moveCreationKey, MoveCreation())
		populateCategoryAndNature()
		setControls()

		move = savedInstanceState.getFromJson(moveKey, Move())
		populateOldData()
	}

	private fun populateScreen(data: MoveCreation) {
		moveCreation = data

		if (data.move != null)
			move = data.move

		move.setDefaultData(accountUrl, data.isUsingCategories)
		populateOldData()
		populateCategoryAndNature()
	}

	private fun populateOldData() {
		var canMove = true

		if (moveCreation.accountList.isEmpty()) {
			canMove = false
			no_accounts.visibility = View.VISIBLE
		}

		if (moveCreation.isUsingCategories) {
			val categoryList = moveCreation.categoryList

			if (categoryList.isNullOrEmpty()) {
				canMove = false
				no_categories.visibility = View.VISIBLE
			} else {
				setDataFromList(categoryList, move.categoryName, category)
			}
		}

		if (!canMove) {
			form.visibility = View.GONE
			warnings.visibility = View.VISIBLE
			return
		}

		if (move.checked) {
			remove_check.visibility = View.VISIBLE;
		}

		date.text = move.date.format()

		moveCreation.natureList.firstOrNull {
			it.value.toInt() == move.natureEnum?.value
		}?.let {
			setNature(it.text, it.value.toInt())
		}

		setDataFromList(moveCreation.accountList, move.outUrl, account_out)
		setDataFromList(moveCreation.accountList, move.inUrl, account_in)

		description.setText(move.description)

		if (move.detailList.isNotEmpty()) {
			useDetailed()

			move.detailList.forEach {
				addViewDetail(move, it.description, it.amount, it.value)
			}
		} else if (move.value != null) {
			useSimple()

			value.setText(String.format("%1$,.2f", move.value))
		}
	}

	private fun setDataFromList(list: Array<ComboItem>, dataSaved: String?, field: TextView) {
		val saved = list.firstOrNull {
			it.value == dataSaved
		} ?: return

		field.text = saved.text
	}

	private fun populateCategoryAndNature() {
		category.visibility =
			if (moveCreation.isUsingCategories)
				View.VISIBLE
			else
				View.GONE

		lose_category.visibility =
			if (move.warnCategory)
				View.VISIBLE
			else
				View.GONE

		if (move.warnCategory)
		{
			lose_category.paintFlags =
				lose_category.paintFlags or
					Paint.STRIKE_THRU_TEXT_FLAG
		}

		if (move.natureEnum == null)
		{
			moveCreation.natureList
				.firstOrNull()?.let {
					setNature(it.text, it.value.toInt())
				}
		}
	}

	private fun setCurrentDate() {
		move.date = Date()
	}

	private fun setControls() {
		date.text = move.date.format()

		description.onChange {
			move.description = it
		}

		value.onChange {
			move.setValue(it)
		}
	}

	fun showDatePicker(@Suppress(ON_CLICK) view: View) {
		dialog.show()
	}

	fun changeCategory(@Suppress(ON_CLICK) view: View) {
		val categoryList = moveCreation.categoryList ?: return
		showChangeList(categoryList, R.string.category) { text, value ->
			category.text = text
			move.categoryName = value
		}
	}

	fun warnLoseCategory(@Suppress(ON_CLICK) view: View) {
		this.alertError(R.string.losingCategory)
	}

	fun changeNature(@Suppress(ON_CLICK) view: View) {
		showChangeList(moveCreation.natureList, R.string.nature) { text, value ->
			setNature(text, value.toInt())
		}
	}

	private fun setNature(text: String, value: Int) {
		nature.text = text
		move.natureEnum = Nature.get(value)

		val accountOutVisibility = if (move.natureEnum != Nature.In) View.VISIBLE else View.GONE
		account_out.visibility = accountOutVisibility

		val accountInVisibility = if (move.natureEnum != Nature.Out) View.VISIBLE else View.GONE
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

	fun changeAccountOut(@Suppress(ON_CLICK) view: View) {
		showChangeList(moveCreation.accountList, R.string.account) { text, value ->
			account_out.text = text
			move.outUrl = value
		}
	}

	fun changeAccountIn(@Suppress(ON_CLICK) view: View) {
		showChangeList(moveCreation.accountList, R.string.account) { text, value ->
			account_in.text = text
			move.inUrl = value
		}
	}

	fun useDetailed(@Suppress(ON_CLICK) view: View) {
		useDetailed()
	}

	private fun useDetailed() {
		move.isDetailed = true

		simple_value.visibility = View.GONE
		detailed_value.visibility = View.VISIBLE

		scrollToTheEnd()
	}

	fun useSimple(@Suppress(ON_CLICK) view: View) {
		useSimple()
	}

	private fun useSimple() {
		move.isDetailed = false

		simple_value.visibility = View.VISIBLE
		detailed_value.visibility = View.GONE

		scrollToTheEnd()
	}

	fun addDetail(@Suppress(ON_CLICK) view: View) {
		val description = detail_description.text.toString()
		val amountStr = detail_amount.text.toString()
		val valueStr = detail_value.text.toString()

		val value = valueStr.toDoubleByCulture()

		if (description.isEmpty() || amountStr.isEmpty() || valueStr.isEmpty() || value == null) {
			alertError(R.string.fill_all)
			return
		}

		val amountDefault = resources.getInteger(R.integer.amount_default)

		detail_description.setText("")
		detail_amount.setText(amountDefault.toString())
		detail_value.setText("")

		val amount = amountStr.toInt()

		move.add(description, amount, value)

		addViewDetail(move, description, amount, value)

		scrollToTheEnd()
	}

	private fun addViewDetail(move: Move?, description: String?, amount: Int, value: Double) {
		val row = DetailBox(this, move, description, amount, value)
		details.addView(row)
	}

	fun save(@Suppress(ON_CLICK) view: View) {
		move.clearNotUsedValues()
		api.saveMove(move, this::cleanAndBack)
	}

	private fun cleanAndBack() {
		intent.removeExtra("id")
		back()
	}

	private fun back() {
		backWithExtras()
	}

	private fun scrollToTheEnd() {
		form.postDelayed({ form.fullScroll(ScrollView.FOCUS_DOWN) }, 100)
	}
}
