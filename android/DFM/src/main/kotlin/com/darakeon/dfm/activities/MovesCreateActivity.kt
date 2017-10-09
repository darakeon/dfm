package com.darakeon.dfm.activities

import android.app.DatePickerDialog
import android.os.Bundle
import android.view.View
import android.widget.*
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.*
import com.darakeon.dfm.activities.objects.MovesCreateStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.api.Step
import com.darakeon.dfm.api.entities.Move
import com.darakeon.dfm.api.entities.Nature
import com.darakeon.dfm.api.toDoubleByCulture
import com.darakeon.dfm.uiHelpers.dialogs.*
import com.darakeon.dfm.uiHelpers.views.DetailBox
import com.darakeon.dfm.uiHelpers.watchers.DescriptionWatcher
import com.darakeon.dfm.uiHelpers.watchers.ValueWatcher
import com.darakeon.dfm.user.GetAuth
import org.json.JSONArray
import org.json.JSONObject
import java.util.*

class MovesCreateActivity : SmartActivity<MovesCreateStatic>(MovesCreateStatic), IDatePickerActivity {
	private val window: ScrollView get() = findViewById(R.id.window)

	override var dialog: DatePickerDialog? = null


	override fun contentView(): Int = R.layout.moves_create


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
			findViewById<TextView>(R.id.no_accounts).visibility = View.VISIBLE
		}

		if (static.useCategories) {
			if (static.categoryList.length() == 0) {
				canMove = false
				findViewById<TextView>(R.id.no_categories).visibility = View.VISIBLE
			} else {
				setDataFromList(static.categoryList, static.move.Category, R.id.category)
			}
		}

		if (!canMove) {
			findViewById<ScrollView>(R.id.window).visibility = View.GONE
			findViewById<LinearLayout>(R.id.warnings).visibility = View.VISIBLE
			return
		}

		setValue(R.id.date, static.move.DateString())

		val list = static.natureList

		for (n in 0 until list.length()) {
			val nature = list.getJSONObject(n)
			val compareValue = nature.getInt("Value")

			if (compareValue == static.move.Nature?.GetNumber()) {
				val text = nature.getString("Text")
				val value = nature.getString("Value")
				setNature(text, value)
				break
			}
		}

		setDataFromList(static.accountList, static.move.AccountOut, R.id.account_out)
		setDataFromList(static.accountList, static.move.AccountIn, R.id.account_in)

		if (static.move.isDetailed) {

			static.move.Details.forEach {
				addViewDetail(static.move, it.Description, it.Amount, it.Value)
			}

			useDetailed()
		}

		if (!populateAll)
			return

		val descriptionView = findViewById<EditText>(R.id.description)
		descriptionView.setText(static.move.Description)

		if (static.move.Value != 0.0) {
			val valueView = findViewById<EditText>(R.id.value)
			valueView.setText(String.format("%1$,.2f", static.move.Value))
		}
	}

	private fun setDataFromList(list: JSONArray?, dataSaved: String?, resourceId: Int) {
		if (list != null) {
			for (n in 0 until list.length()) {
				val `object` = list.getJSONObject(n)
				val value = `object`.getString("Value")

				if (value == dataSaved) {
					val text = `object`.getString("Text")
					setValue(resourceId, text)
					break
				}
			}
		}
	}

	private fun populateScreen() {
		val request = InternalRequest(this, "Moves/Create")

		request.AddParameter("ticket", GetAuth())
		request.AddParameter("accountUrl", intent.getStringExtra("accountUrl"))
		request.AddParameter("id", intent.getIntExtra("id", 0))
		request.Get(Step.Populate)

		setCurrentDate()
		setControls()
	}

	override fun HandleSuccess(data: JSONObject, step: Step) {
		when (step) {
			Step.Populate -> {
				populateScreen(data)
			}
			Step.Recording -> {
				intent.removeExtra("id")
				back()
			}
			else -> {
				alertError(R.string.this_is_not_happening)
			}
		}
	}

	private fun populateScreen(data: JSONObject) {
		static.useCategories = data.getBoolean("UseCategories")

		if (static.useCategories)
			static.categoryList = data.getJSONArray("CategoryList")

		static.natureList = data.getJSONArray("NatureList")
		static.accountList = data.getJSONArray("AccountList")

		populateCategoryAndNature()

		if (data.has("Move") && !data.isNull("Move")) {
			val moveToEdit = data.getJSONObject("Move")

			val accountUrl =
				if (intent.hasExtra("accountUrl"))
					intent.getStringExtra("accountUrl")
				else
					null

			static.move.SetData(moveToEdit, accountUrl)
			populateOldData(true)
		}
	}

	private fun populateCategoryAndNature() {

		findViewById<Button>(R.id.category).visibility =
			if (static.useCategories)
				View.VISIBLE
			else
				View.GONE

		if (static.move.Nature == null) {
			val firstNature = static.natureList.getJSONObject(0)
			setValue(R.id.nature, firstNature.getString("Text"))
			static.move.SetNature(firstNature.getInt("Value"))
		}
	}


	private fun setCurrentDate() {
		val today = Calendar.getInstance()
		val day = intent.getIntExtra("day", today.get(Calendar.DAY_OF_MONTH))
		val month = intent.getIntExtra("month", today.get(Calendar.MONTH))
		val year = intent.getIntExtra("year", today.get(Calendar.YEAR))
		static.move.Date.set(year, month, day)
	}

	private fun setControls() {
		setValue(R.id.date, static.move.DateString())

		setDescriptionListener()
		setValueListener()
	}

	private fun setDescriptionListener() {
		val textMessage = findViewById<EditText>(R.id.description)
		textMessage.addTextChangedListener(DescriptionWatcher(static.move))
	}

	private fun setValueListener() {
		val textMessage = findViewById<EditText>(R.id.value)
		textMessage.addTextChangedListener(ValueWatcher(static.move))
	}


	fun showDatePicker(@Suppress(onClick) view: View) {
		dialog = DatePickerDialog(
			this, PickDate(this),
				static.move.year, static.move.month, static.move.day
		)

		dialog?.show()
	}

	override fun setResult(year: Int, month: Int, day: Int) {
		static.move.Date.set(year, month, day)
		setValue(R.id.date, static.move.DateString())
	}


	fun changeCategory(@Suppress(onClick) view: View) {
		showChangeList(static.categoryList, R.string.category, DialogCategory(static.categoryList, this, static.move))
	}

	fun changeNature(@Suppress(onClick) view: View) {
		showChangeList(static.natureList, R.string.nature, DialogNature(static.natureList, this))
	}


	fun setNature(text: String, value: String) {
		setValue(R.id.nature, text)
		static.move.SetNature(value)

		val accountOutVisibility = if (static.move.Nature != Nature.In) View.VISIBLE else View.GONE
		findViewById<Button>(R.id.account_out).visibility = accountOutVisibility

		val accountInVisibility = if (static.move.Nature != Nature.Out) View.VISIBLE else View.GONE
		findViewById<Button>(R.id.account_in).visibility = accountInVisibility

		if (static.move.Nature == Nature.Out) {
			static.move.AccountIn = null
			setValue(R.id.account_in, getString(R.string.account_in))
		}

		if (static.move.Nature == Nature.In) {
			static.move.AccountOut = null
			setValue(R.id.account_out, getString(R.string.account_out))
		}
	}


	fun changeAccountOut(@Suppress(onClick) view: View) {
		showChangeList(static.accountList, R.string.account, DialogAccountOut(static.accountList, this, static.move))
	}

	fun changeAccountIn(@Suppress(onClick) view: View) {
		showChangeList(static.accountList, R.string.account, DialogAccountIn(static.accountList, this, static.move))
	}

	fun useDetailed(@Suppress(onClick) view: View) {
		useDetailed()
	}

	fun useDetailed() {
		static.move.isDetailed = true

		findViewById<LinearLayout>(R.id.simple_value).visibility = View.GONE
		findViewById<LinearLayout>(R.id.detailed_value).visibility = View.VISIBLE

		scrollToTheEnd()
	}

	fun useSimple(@Suppress(onClick) view: View) {
		static.move.isDetailed = false

		findViewById<LinearLayout>(R.id.simple_value).visibility = View.VISIBLE
		findViewById<LinearLayout>(R.id.detailed_value).visibility = View.GONE

		scrollToTheEnd()
	}

	fun addDetail(@Suppress(onClick) view: View) {
		val description = getValue(R.id.detail_description)
		val amountStr = getValue(R.id.detail_amount)
		val valueStr = getValue(R.id.detail_value)

		val value = valueStr.toDoubleByCulture()

		if (description.isEmpty() || amountStr.isEmpty() || valueStr.isEmpty() || value == null) {
			alertError(R.string.fill_all)
			return
		}

		val amountDefault = resources.getInteger(R.integer.amount_default)

		setValue(R.id.detail_description, "")
		setValue(R.id.detail_amount, amountDefault)
		setValue(R.id.detail_value, "")

		val amount = amountStr.toInt()

		static.move.Add(description, amount, value)

		addViewDetail(static.move, description, amount, value)

		scrollToTheEnd()
	}

	private fun addViewDetail(move: Move?, description: String?, amount: Int, value: Double) {
		val row = DetailBox(this, move, description, amount, value)
		val list = findViewById<LinearLayout>(R.id.details)
		list.addView(row)
	}


	fun save(@Suppress(onClick) view: View) {
		val request = InternalRequest(this, "Moves/Create")

		request.AddParameter("ticket", GetAuth())
		static.move.setParameters(request)

		request.Post(Step.Recording)
	}

	private fun back() {
		navigation.redirectWithExtras()
	}


	private fun scrollToTheEnd() {
		window.postDelayed({ window.fullScroll(ScrollView.FOCUS_DOWN) }, 100)
	}


}

