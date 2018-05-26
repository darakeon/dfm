package com.darakeon.dfm.moves

import android.app.DatePickerDialog
import android.graphics.Paint
import android.os.Bundle
import android.view.View
import android.widget.ScrollView
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.api.entities.Move
import com.darakeon.dfm.api.entities.Nature
import com.darakeon.dfm.api.toDoubleByCulture
import com.darakeon.dfm.auth.getAuth
import com.darakeon.dfm.base.SmartActivity
import com.darakeon.dfm.dialogs.alertError
import com.darakeon.dfm.dialogs.getDateDialog
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.redirectWithExtras
import com.darakeon.dfm.extensions.showChangeList
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
import kotlinx.android.synthetic.main.moves_create.simple_value
import kotlinx.android.synthetic.main.moves_create.value
import kotlinx.android.synthetic.main.moves_create.warnings
import org.json.JSONArray
import org.json.JSONObject
import java.util.Calendar

class MovesCreateActivity : SmartActivity<MovesCreateStatic>(MovesCreateStatic) {
	private val dialog: DatePickerDialog
		get() {
			val ( year, month, day ) = static.move

			return getDateDialog(
				{ y, m, d -> updateDateCombo(y, m, d) },
				year, month, day
			)
		}

	override fun contentView(): Int = R.layout.moves_create

	private fun updateDateCombo(year: Int, month: Int, day: Int) {
		static.move.date.set(year, month, day)
		date.text = static.move.dateString()
	}

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (rotated && succeeded) {
			populateCategoryAndNature()
			setControls()
			populateOldData(false)
		} else {
			static.move = Move()
			populateScreen()
		}
	}

	private fun populateOldData(populateAll: Boolean) {

		var canMove = true

		if (static.accountList.length() == 0) {
			canMove = false
			no_accounts.visibility = View.VISIBLE
		}

		if (static.useCategories) {
			if (static.categoryList.length() == 0) {
				canMove = false
				no_categories.visibility = View.VISIBLE
			} else {
				setDataFromList(static.categoryList, static.move.category, category)
			}
		}

		if (!canMove) {
			form.visibility = View.GONE
			warnings.visibility = View.VISIBLE
			return
		}

		date.text = static.move.dateString()

		val list = static.natureList

		for (n in 0 until list.length()) {
			val nature = list.getJSONObject(n)
			val compareValue = nature.getInt("Value")

			if (compareValue == static.move.nature?.getNumber()) {
				val text = nature.getString("Text")
				val value = nature.getString("Value")
				setNature(text, value)
				break
			}
		}

		setDataFromList(static.accountList, static.move.accountOut, account_out)
		setDataFromList(static.accountList, static.move.accountIn, account_in)

		if (static.move.isDetailed) {

			static.move.details.forEach {
				addViewDetail(static.move, it.description, it.amount, it.value)
			}

			useDetailed()
		}

		if (!populateAll)
			return

		description.setText(static.move.description)

		if (static.move.value != 0.0) {
			value.setText(String.format("%1$,.2f", static.move.value))
		}
	}

	private fun setDataFromList(list: JSONArray?, dataSaved: String?, field: TextView) {
		if (list != null) {
			for (n in 0 until list.length()) {
				val item = list.getJSONObject(n)
				val value = item.getString("Value")

				if (value == dataSaved) {
					field.text = item.getString("Text")
					break
				}
			}
		}
	}

	private fun populateScreen() {
		val request = InternalRequest(
			this, "Moves/Create", { d -> populateScreen(d) }
		)

		request.addParameter("ticket", getAuth())
		request.addParameter("accountUrl", getExtraOrUrl("accountUrl"))
		request.addParameter("id", getExtraOrUrl("id", null))
		request.get()

		setCurrentDate()
		setControls()
	}

	private fun populateScreen(data: JSONObject) {
		static.useCategories = data.getBoolean("UseCategories")

		if (static.useCategories)
			static.categoryList = data.getJSONArray("CategoryList")

		static.natureList = data.getJSONArray("NatureList")
		static.accountList = data.getJSONArray("AccountList")

		if (data.has("Move") && !data.isNull("Move")) {
			val moveToEdit = data.getJSONObject("Move")

			val accountUrl = getExtraOrUrl("accountUrl")

			static.move.setData(moveToEdit, accountUrl, static.useCategories)
			populateOldData(true)
		}

		populateCategoryAndNature()
	}

	private fun populateCategoryAndNature()
	{
		category.visibility =
			if (static.useCategories)
				View.VISIBLE
			else
				View.GONE

		lose_category.visibility =
			if (static.move.warnCategory)
				View.VISIBLE
			else
				View.GONE

		if (static.move.warnCategory)
		{
			lose_category.paintFlags =
				lose_category.paintFlags or
					Paint.STRIKE_THRU_TEXT_FLAG
		}

		if (static.move.nature == null)
		{
			val firstNature = static.natureList.getJSONObject(0)
			val text = firstNature.getString("Text")
			val value = firstNature.getInt("Value")
			setNature(text, value)
		}
	}


	private fun setCurrentDate() {
		val today = Calendar.getInstance()
		val day = intent.getIntExtra("day", today.get(Calendar.DAY_OF_MONTH))
		val month = intent.getIntExtra("month", today.get(Calendar.MONTH))
		val year = intent.getIntExtra("year", today.get(Calendar.YEAR))
		static.move.date.set(year, month, day)
	}

	private fun setControls() {
		date.text = static.move.dateString()

		setDescriptionListener()
		setValueListener()
	}

	private fun setDescriptionListener() {
		description.addTextChangedListener(DescriptionWatcher(static.move))
	}

	private fun setValueListener() {
		value.addTextChangedListener(ValueWatcher(static.move))
	}


	fun showDatePicker(@Suppress(ON_CLICK) view: View) {
		dialog.show()
	}

	fun changeCategory(@Suppress(ON_CLICK) view: View) {
		showChangeList(static.categoryList, R.string.category, { t, v -> setCategory(t, v) })
	}

	fun warnLoseCategory(@Suppress(ON_CLICK) view: View) {
		this.alertError(R.string.losingCategory)
	}

	private fun setCategory(text: String, value: String) {
		category.text = text
		static.move.category = value
	}

	fun changeNature(@Suppress(ON_CLICK) view: View) {
		showChangeList(static.natureList, R.string.nature, { t, v -> setNature(t, v) })
	}

	private fun setNature(text: String, value: String) {
		setNature(text, value.toInt())
	}

	private fun setNature(text: String, value: Int?) {
		nature.text = text
		static.move.setNature(value)

		val accountOutVisibility = if (static.move.nature != Nature.In) View.VISIBLE else View.GONE
		account_out.visibility = accountOutVisibility

		val accountInVisibility = if (static.move.nature != Nature.Out) View.VISIBLE else View.GONE
		account_in.visibility = accountInVisibility

		if (static.move.nature == Nature.Out) {
			static.move.accountIn = null
			account_in.text = getString(R.string.account_in)
		}

		if (static.move.nature == Nature.In) {
			static.move.accountOut = null
			account_out.text = getString(R.string.account_out)
		}
	}


	fun changeAccountOut(@Suppress(ON_CLICK) view: View) {
		showChangeList(static.accountList, R.string.account, { t,v -> setAccountOut(t, v) })
	}

	private fun setAccountOut(text: String, value: String) {
		account_out.text = text
		static.move.accountOut = value
	}

	fun changeAccountIn(@Suppress(ON_CLICK) view: View) {
		showChangeList(static.accountList, R.string.account, { t, v -> setAccountIn(t, v) })
	}

	private fun setAccountIn(text: String, value: String) {
		account_in.text = text
		static.move.accountIn = value
	}

	fun useDetailed(@Suppress(ON_CLICK) view: View) {
		useDetailed()
	}

	private fun useDetailed() {
		static.move.isDetailed = true

		simple_value.visibility = View.GONE
		detailed_value.visibility = View.VISIBLE

		scrollToTheEnd()
	}

	fun useSimple(@Suppress(ON_CLICK) view: View) {
		static.move.isDetailed = false

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

		static.move.add(description, amount, value)

		addViewDetail(static.move, description, amount, value)

		scrollToTheEnd()
	}

	private fun addViewDetail(move: Move?, description: String?, amount: Int, value: Double) {
		val row = DetailBox(this, move, description, amount, value)
		details.addView(row)
	}


	fun save(@Suppress(ON_CLICK) view: View) {
		val request = InternalRequest(
			this, "Moves/Create", { cleanAndBack() }
		)

		request.addParameter("ticket", getAuth())
		static.move.setParameters(request)

		request.post()
	}

	private fun cleanAndBack() {
		intent.removeExtra("id")
		back()
	}

	private fun back() {
		redirectWithExtras()
	}


	private fun scrollToTheEnd() {
		form.postDelayed({ form.fullScroll(ScrollView.FOCUS_DOWN) }, 100)
	}


}

