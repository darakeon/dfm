package com.darakeon.dfm.extract

import android.app.DatePickerDialog
import android.os.Bundle
import android.view.ContextMenu
import android.view.MenuItem
import android.view.View
import android.widget.ListView
import com.darakeon.dfm.R
import com.darakeon.dfm.api.old.InternalRequest
import com.darakeon.dfm.auth.auth
import com.darakeon.dfm.auth.highLightColor
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.dialogs.IYesNoDialogAnswer
import com.darakeon.dfm.dialogs.alertYesNo
import com.darakeon.dfm.dialogs.getDateDialog
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.createMove
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.extensions.setValueColored
import kotlinx.android.synthetic.main.extract.empty_list
import kotlinx.android.synthetic.main.extract.main_table
import kotlinx.android.synthetic.main.extract.reportChange
import kotlinx.android.synthetic.main.extract.total_title
import kotlinx.android.synthetic.main.extract.total_value
import org.json.JSONObject
import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Locale

class ExtractActivity : BaseActivity<ExtractStatic>(ExtractStatic), IYesNoDialogAnswer {
	private val accountUrl: String get() = getExtraOrUrl("accountUrl")

	private val dialog: DatePickerDialog
		get() = getDateDialog(
			{ y, m, _ -> updateScreen(y, m) },
			static.year, static.month
		)

	override val contentView = R.layout.extract
	override val contextMenuResource = R.menu.move_options
	override val viewWithContext: ListView get() = main_table

	private fun updateScreen(year: Int, month: Int) {
		setDate(month, year)
		getExtract()
	}

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		highlight?.setBackgroundColor(highLightColor)

		if (rotated && static.succeeded) {
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
			val startMonth = id.rem(100) - 1
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

		reportChange.text = dateInFull
	}

	fun changeDate(@Suppress(ON_CLICK) view: View) {
		dialog.show()
	}

	private fun getExtract() {
		val request = InternalRequest(
			this, "Moves/Extract", { d -> handleMoves(d) }
		)

		request.addParameter("ticket", auth)
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
		total_title.text = static.name
		setValueColored(total_value, static.total)

		if (static.moveList.length() == 0) {
			main_table.visibility = View.GONE
			empty_list.visibility = View.VISIBLE
		} else {
			main_table.visibility = View.VISIBLE
			empty_list.visibility = View.GONE

			val accountAdapter = MoveAdapter(this, static.moveList, static.canCheck)
			main_table.adapter = accountAdapter
		}
	}


	fun goToSummary(@Suppress(ON_CLICK) view: View) {
		redirect<ExtractActivity> {
			it.putExtra("accountUrl", accountUrl)
			it.putExtra("year", static.year)
		}
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

		request.addParameter("ticket", auth)
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

