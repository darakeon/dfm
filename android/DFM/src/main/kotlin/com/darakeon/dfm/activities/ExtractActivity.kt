package com.darakeon.dfm.activities

import android.app.DatePickerDialog
import android.content.Intent
import android.os.Bundle
import android.view.ContextMenu
import android.view.MenuItem
import android.view.View
import android.widget.ListView
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.*
import com.darakeon.dfm.activities.objects.ExtractStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.uiHelpers.adapters.MoveAdapter
import com.darakeon.dfm.uiHelpers.views.MoveLine
import com.darakeon.dfm.user.getAuth
import org.json.JSONObject
import java.text.SimpleDateFormat
import java.util.*

class ExtractActivity : SmartActivity<ExtractStatic>(ExtractStatic), IYesNoDialogAnswer {
	private val main: ListView get() = findViewById(R.id.main_table)
	private val empty: TextView get() = findViewById(R.id.empty_list)

	private val accountUrl: String get() = getExtraOrUrl("accountUrl")

	private val dialog: DatePickerDialog
		get() = getDateDialog(
			{ y, m, _ -> updateScreen(y, m) },
			static.year, static.month
		)

	override fun contentView(): Int = R.layout.extract
	override fun contextMenuResource(): Int = R.menu.move_options
	override fun viewWithContext(): Int = R.id.main_table

	private fun updateScreen(year: Int, month: Int) {
		setDate(month, year)
		getExtract()
	}

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		if (rotated && succeeded) {
			setDateFromLast()
			fillMoves()
		} else {
			setDateFromCaller()
			getExtract()
		}
	}

	private fun setDateFromLast() {
		setDate(static.month, static.year)
	}

	private fun setDateFromCaller() {
		if (query.containsKey("id")) {
			val id = query["id"]?.toInt() ?: 0
			val startMonth = id.mod(100) - 1
			val startYear = id / 100
			setDate(startMonth, startYear)
		} else {
			val today = Calendar.getInstance()
			val startMonth = intent.getIntExtra("month", today.get(Calendar.MONTH))
			val startYear = intent.getIntExtra("year", today.get(Calendar.YEAR))
			setDate(startMonth, startYear)
		}
	}

	private fun setDate(month: Int, year: Int) {
		static.month = month
		static.year = year

		intent.putExtra("month", month)
		intent.putExtra("year", year)

		val date = Calendar.getInstance()
		date.set(Calendar.MONTH, month)
		date.set(Calendar.YEAR, year)

		val formatter = SimpleDateFormat("MMM/yyyy", Locale.getDefault())
		val dateInFull = formatter.format(date.time)

		setValue(R.id.reportChange, dateInFull)
	}

	fun changeDate(@Suppress(ON_CLICK) view: View) {
		dialog.show()
	}

	private fun getExtract() {
		val request = InternalRequest(
			this, "Moves/Extract", { d -> handleMoves(d) }
		)

		request.addParameter("ticket", getAuth())
		request.addParameter("accountUrl", accountUrl)
		request.addParameter("id", static.year * 100 + static.month + 1)

		request.post()
	}

	private fun handleMoves(data: JSONObject) {
		static.moveList = data.getJSONArray("MoveList")
		static.name = data.getString("Name")
		static.total = data.getDouble("Total")
		static.canCheck = data.getBoolean("CanCheck")

		fillMoves()
	}

	private fun fillMoves() {
		setValue(R.id.totalTitle, static.name)
		setValueColored(R.id.totalValue, static.total)

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


	fun goToSummary(@Suppress(ON_CLICK) view: View) {
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


	private fun askDelete(): Boolean {
		var messageText = getString(R.string.sure_to_delete)
		val moveRow = clickedView as MoveLine

		messageText = String.format(messageText, moveRow.description)

		alertYesNo(messageText, this)

		return false
	}

	override fun yesAction() {
		submitMoveAction("Delete")
	}

	override fun noAction() {}


	private fun check() {
		submitMoveAction("Check")
	}

	private fun uncheck() {
		submitMoveAction("Uncheck")
	}


	private fun submitMoveAction(action: String) {
		val view = clickedView as MoveLine

		val request = InternalRequest(
			this, "Moves/" + action, { refresh() }
		)

		request.addParameter("ticket", getAuth())
		request.addParameter("accountUrl", accountUrl)
		request.addParameter("id", view.id)

		request.post()
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

	private fun hideMenuItem(menu: ContextMenu, id: Int) {
		toggleMenuItem(menu, id, false)
	}

	private fun showMenuItem(menu: ContextMenu, id: Int) {
		toggleMenuItem(menu, id, true)
	}

	private fun toggleMenuItem(menu: ContextMenu, id: Int, show: Boolean) {
		val buttonToHide = menu.findItem(id)
		buttonToHide.isVisible = show
	}

}

