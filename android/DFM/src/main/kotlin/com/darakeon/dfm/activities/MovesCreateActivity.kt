package com.darakeon.dfm.activities

import android.app.DatePickerDialog
import android.os.Bundle
import android.view.View
import android.widget.EditText
import android.widget.LinearLayout
import android.widget.ScrollView
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.objects.MovesCreateStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.api.Step
import com.darakeon.dfm.api.entities.Move
import com.darakeon.dfm.api.entities.Nature
import com.darakeon.dfm.uiHelpers.dialogs.*
import com.darakeon.dfm.uiHelpers.views.DetailBox
import com.darakeon.dfm.uiHelpers.watchers.DescriptionWatcher
import com.darakeon.dfm.uiHelpers.watchers.ValueWatcher
import org.json.JSONArray
import org.json.JSONException
import org.json.JSONObject
import java.util.*

class MovesCreateActivity : SmartActivity<MovesCreateStatic>(MovesCreateStatic), IDatePickerActivity {
	internal val window: ScrollView get() = findViewById(R.id.window) as ScrollView

	override var dialog: DatePickerDialog? = null


	override fun contentView(): Int {
		return R.layout.moves_create
	}


	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)


		if (rotated && succeeded) {
			try {
				populateCategoryAndNature()
				setControls()
				populateOldData(false)
			} catch (e: JSONException) {
				message.alertError(R.string.error_activity_json, e)
			}

		} else {
			static.move = Move()
			populateScreen()
		}
	}

	@Throws(NumberFormatException::class, JSONException::class)
	private fun populateOldData(populateAll: Boolean) {
		if (static.useCategories) {
			setDataFromList(static.categoryList, static.move.Category, R.id.category)
		}

		form.setValue(R.id.date, static.move.DateString())

		val list = static.natureList

		if (list != null) {
			for (n in 0..list.length() - 1) {
				val nature = list.getJSONObject(n)
				val comparValue = nature.getInt("Value")

				if (comparValue == static.move.Nature?.GetNumber()) {
					val text = nature.getString("Text")
					val value = nature.getString("Value")
					setNature(text, value)
					break
				}
			}
		}

		setDataFromList(static.accountList, static.move.AccountOut, R.id.account_out)
		setDataFromList(static.accountList, static.move.AccountIn, R.id.account_in)

		if (static.move.isDetailed) {
			for (d in static.move.Details.indices) {
				val detail = static.move.Details[d]

				addViewDetail(static.move, detail.Description, detail.Amount, detail.Value)
			}

			useDetailed()
		}

		if (!populateAll)
			return

		val descriptionView = findViewById(R.id.description) as EditText
		descriptionView.setText(static.move.Description)

		if (static.move.Value != 0.0) {
			val valueView = findViewById(R.id.value) as EditText
			valueView.setText(String.format("%1$,.2f", static.move.Value))
		}
	}

	@Throws(JSONException::class)
	private fun setDataFromList(list: JSONArray?, dataSaved: String?, resourceId: Int) {
		if (list != null) {
			for (n in 0..list.length() - 1) {
				val `object` = list.getJSONObject(n)
				val value = `object`.getString("Value")

				if (value == dataSaved) {
					val text = `object`.getString("Text")
					form.setValue(resourceId, text)
					break
				}
			}
		}
	}

	private fun populateScreen() {
		val request = InternalRequest(this, "Moves/Create")

		request.AddParameter("ticket", Authentication.Get())
		request.AddParameter("accountUrl", intent.getStringExtra("accountUrl"))
		request.AddParameter("id", intent.getIntExtra("id", 0))
		request.Get(Step.Populate)

		setCurrentDate()
		setControls()
	}

	@Throws(JSONException::class)
	override fun HandleSuccess(data: JSONObject, step: Step) {
		when (step) {
			Step.Populate -> {
				populateScreen(data)
			}
			Step.Recording -> {
				back()
			}
			else -> {
				message.alertError(R.string.this_is_not_happening)
			}
		}
	}

	@Throws(JSONException::class)
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

	@Throws(JSONException::class)
	private fun populateCategoryAndNature() {

		findViewById(R.id.category).visibility =
			if (static.useCategories)
				View.VISIBLE
			else
				View.GONE

		if (static.move.Nature == null) {
			val firstNature = static.natureList?.getJSONObject(0)
			form.setValue(R.id.nature, firstNature?.getString("Text"))
			static.move.SetNature(firstNature?.getInt("Value"))
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
		form.setValue(R.id.date, static.move.DateString())

		setDescriptionListener()
		setValueListener()
	}

	private fun setDescriptionListener() {
		val textMessage = findViewById(R.id.description) as EditText
		textMessage.addTextChangedListener(DescriptionWatcher(static.move))
	}

	private fun setValueListener() {
		val textMessage = findViewById(R.id.value) as EditText
		textMessage.addTextChangedListener(ValueWatcher(static.move))
	}


	fun showDatePicker(view: View) {
		dialog = DatePickerDialog(
			this, PickDate(this),
				static.move.year, static.move.month, static.move.day
		)

		dialog?.show()
	}

	override fun setResult(year: Int, month: Int, day: Int) {
		static.move.Date.set(year, month, day)
		form.setValue(R.id.date, static.move.DateString())
	}


	fun changeCategory(view: View) {
		form.showChangeList(static.categoryList, R.string.category, DialogCategory(static.categoryList, form, message, static.move))
	}

	fun changeNature(view: View) {
		form.showChangeList(static.natureList, R.string.nature, DialogNature(static.natureList, this))
	}


	fun setNature(text: String, value: String) {
		form.setValue(R.id.nature, text)
		static.move.SetNature(value)

		val accountOutVisibility = if (static.move.Nature != Nature.In) View.VISIBLE else View.GONE
		findViewById(R.id.account_out).visibility = accountOutVisibility

		val accountInVisibility = if (static.move.Nature != Nature.Out) View.VISIBLE else View.GONE
		findViewById(R.id.account_in).visibility = accountInVisibility

		if (static.move.Nature == Nature.Out) {
			static.move.AccountIn = null
			form.setValue(R.id.account_in, getString(R.string.account_in))
		}

		if (static.move.Nature == Nature.In) {
			static.move.AccountOut = null
			form.setValue(R.id.account_out, getString(R.string.account_out))
		}
	}


	fun changeAccountOut(view: View) {
		form.showChangeList(static.accountList, R.string.account, DialogAccountOut(static.accountList, form, message, static.move))
	}

	fun changeAccountIn(view: View) {
		form.showChangeList(static.accountList, R.string.account, DialogAccountIn(static.accountList, form, message, static.move))
	}

	@JvmOverloads fun useDetailed(view: View? = null) {
		static.move.isDetailed = true

		findViewById(R.id.simple_value).visibility = View.GONE
		findViewById(R.id.detailed_value).visibility = View.VISIBLE

		scrollToTheEnd()
	}

	fun useSimple(view: View) {
		static.move.isDetailed = false

		findViewById(R.id.simple_value).visibility = View.VISIBLE
		findViewById(R.id.detailed_value).visibility = View.GONE

		scrollToTheEnd()
	}

	fun addDetail(view: View) {
		val description = form.getValue(R.id.detail_description)
		val amountStr = form.getValue(R.id.detail_amount)
		val valueStr = form.getValue(R.id.detail_value)

		if (description.isEmpty() || amountStr.isEmpty() || valueStr.isEmpty()) {
			message.alertError(R.string.fill_all)
			return
		}

		val amountDefault = resources.getInteger(R.integer.amount_default)

		form.setValue(R.id.detail_description, "")
		form.setValue(R.id.detail_amount, amountDefault)
		form.setValue(R.id.detail_value, "")

		val amount = Integer.parseInt(amountStr)
		val value = java.lang.Double.parseDouble(valueStr)

		static.move.Add(description, amount, value)

		addViewDetail(static.move, description, amount, value)

		scrollToTheEnd()
	}

	private fun addViewDetail(move: Move?, description: String?, amount: Int, value: Double) {
		val row = DetailBox(this, move, description, amount, value)
		val list = findViewById(R.id.details) as LinearLayout
		list.addView(row)
	}


	fun save(view: View) {
		val request = InternalRequest(this, "Moves/Create")

		request.AddParameter("ticket", Authentication.Get())
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

