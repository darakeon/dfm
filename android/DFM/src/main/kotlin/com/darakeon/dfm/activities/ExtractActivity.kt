package com.darakeon.dfm.activities

import android.annotation.SuppressLint
import android.app.DatePickerDialog
import android.content.Intent
import android.os.Bundle
import android.view.ContextMenu
import android.view.MenuItem
import android.view.View
import android.widget.ListView
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.IYesNoDialogAnswer
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.objects.ExtractStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.api.Step
import com.darakeon.dfm.uiHelpers.adapters.MoveAdapter
import com.darakeon.dfm.uiHelpers.dialogs.IDatePickerActivity
import com.darakeon.dfm.uiHelpers.dialogs.PickDate
import com.darakeon.dfm.uiHelpers.views.MoveLine
import org.json.JSONException
import org.json.JSONObject
import java.text.SimpleDateFormat
import java.util.*

class ExtractActivity : SmartActivity<ExtractStatic>(ExtractStatic), IYesNoDialogAnswer, IDatePickerActivity {
	internal val main: ListView get() = findViewById(R.id.main_table) as ListView
	internal val empty: TextView get() = findViewById(R.id.empty_list) as TextView

	internal val accountUrl: String get() = intent.getStringExtra("accountUrl")
	override var dialog: DatePickerDialog? = null


	override fun contentView(): Int {
		return R.layout.extract
	}

	override fun contextMenuResource(): Int {
		return R.menu.move_options
	}

	override fun viewWithContext(): Int {
		return R.id.main_table
	}


	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (rotated && succeeded) {
			setDateFromLast()

			try {
				fillMoves()
			} catch (e: JSONException) {
				message.alertError(R.string.error_activity_json, e)
			}

		} else {
			setDateFromCaller()
			getExtract()
		}
	}

	private fun setDateFromLast() {
		setDate(static.month, static.year)
	}

	private fun setDateFromCaller() {
		val today = Calendar.getInstance()
		val startMonth = intent.getIntExtra("month", today.get(Calendar.MONTH))
		val startYear = intent.getIntExtra("year", today.get(Calendar.YEAR))
		setDate(startMonth, startYear)
	}

	@SuppressLint("SimpleDateFormat")
	private fun setDate(month: Int, year: Int) {
		static.month = month
		static.year = year

		intent.putExtra("month", month)
		intent.putExtra("year", year)

		val date = Calendar.getInstance()
		date.set(Calendar.MONTH, month)
		date.set(Calendar.YEAR, year)

		val formatter = SimpleDateFormat("MMM/yyyy")
		val dateInFull = formatter.format(date.time)

		form.setValue(R.id.reportChange, dateInFull)
	}

	fun changeDate(v: View) {
		if (dialog == null) {
			dialog = DatePickerDialog(this, PickDate(this), static.year, static.month, 1)

			try {
				val pickerField = dialog!!.javaClass.getDeclaredField("mDatePicker")
				pickerField.isAccessible = true

				val field = pickerField.get(dialog).javaClass.getDeclaredField("mDaySpinner")
				field.isAccessible = true
				val dayPicker = field.get(pickerField.get(dialog))
				(dayPicker as View).visibility = View.GONE
			} catch (ignored: Exception) {
			}
		}

		dialog?.show()
	}

	override fun setResult(year: Int, month: Int, day: Int) {
		setDate(month, year)
		getExtract()
	}

	private fun getExtract() {
		val request = InternalRequest(this, "Moves/Extract")

		request.AddParameter("ticket", Authentication.Get())
		request.AddParameter("accountUrl", accountUrl)
		request.AddParameter("id", static.year * 100 + static.month + 1)

		request.Post(Step.Populate)
	}

	@Throws(JSONException::class)
	override fun HandleSuccess(data: JSONObject, step: Step) {
		when (step) {
			Step.Populate -> {
				static.moveList = data.getJSONArray("MoveList")
				static.name = data.getString("Name")
				static.total = data.getDouble("Total")
				static.canCheck = data.getBoolean("CanCheck")

				fillMoves()
			}
			Step.Recording -> {
				refresh()
			}
			else -> {
				message.alertError(R.string.this_is_not_happening)
			}
		}
	}

	@Throws(JSONException::class)
	private fun fillMoves() {
		form.setValue(R.id.totalTitle, static.name)
		form.setValueColored(R.id.totalValue, static.total)

		if (static.moveList.length() == 0) {
			main.visibility = View.GONE
			empty.visibility = View.VISIBLE
		} else {
			main.visibility = View.VISIBLE
			empty.visibility = View.GONE

			val accountAdapter = MoveAdapter(this, static.moveList, static.canCheck)
			main.adapter = accountAdapter
		}
	}


	fun goToSummary(view: View) {
		val intent = Intent(this, SummaryActivity::class.java)

		intent.putExtra("accountUrl", accountUrl)
		intent.putExtra("year", static.year)

		startActivity(intent)
	}

	private fun goToMove(moveId: Int) {

		val extras = Bundle()

		extras.putInt("id", moveId)
		extras.putString("accountUrl", accountUrl)
		extras.putInt("year", static.year)
		extras.putInt("month", static.month)

		createMove(extras)
	}


	override fun onContextItemSelected(item: MenuItem): Boolean {
		when (item.itemId) {
			R.id.edit_move -> {
				edit()
				return true
			}
			R.id.delete_move -> {
				askDelete()
				return true
			}
			R.id.check_move -> {
				check()
				return true
			}
			R.id.uncheck_move -> {
				uncheck()
				return true
			}
			else -> return super.onContextItemSelected(item)
		}
	}

	private fun edit() {
		val moveLine = clickedView as MoveLine
		goToMove(moveLine.id)
	}


	fun askDelete(): Boolean {
		var messageText = getString(R.string.sure_to_delete)
		val moveRow = clickedView as MoveLine

		messageText = String.format(messageText, moveRow.description)

		message.alertYesNo(messageText, this)

		return false
	}

	override fun YesAction() {
		submitMoveAction("Delete")
	}

	override fun NoAction() {}


	private fun check() {
		submitMoveAction("Check")
	}

	private fun uncheck() {
		submitMoveAction("Uncheck")
	}


	private fun submitMoveAction(action: String) {
		val view = clickedView as MoveLine

		val request = InternalRequest(this, "Moves/" + action)

		request.AddParameter("ticket", Authentication.Get())
		request.AddParameter("accountUrl", accountUrl)
		request.AddParameter("id", view.id)

		request.Post(Step.Recording)
	}


	public override fun changeContextMenu(view: View, menuInfo: ContextMenu) {
		val move = clickedView as MoveLine

		try {
			if (static.canCheck) {
				hideMenuItem(menuInfo, if (move.isChecked) R.id.check_move else R.id.uncheck_move)
				showMenuItem(menuInfo, if (move.isChecked) R.id.uncheck_move else R.id.check_move)
			} else {
				hideMenuItem(menuInfo, R.id.check_move)
				hideMenuItem(menuInfo, R.id.uncheck_move)
			}
		} catch (e: Exception) {
			e.printStackTrace()
		}

	}

	fun hideMenuItem(menu: ContextMenu, id: Int) {
		toggleMenuItem(menu, id, false)
	}

	fun showMenuItem(menu: ContextMenu, id: Int) {
		toggleMenuItem(menu, id, true)
	}

	fun toggleMenuItem(menu: ContextMenu, id: Int, show: Boolean) {
		val buttonToHide = menu.findItem(id)
		buttonToHide.isVisible = show
	}

}

