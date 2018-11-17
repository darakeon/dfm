package com.darakeon.dfm.extract

import android.app.DatePickerDialog
import android.os.Bundle
import android.view.ContextMenu
import android.view.MenuItem
import android.view.View
import android.widget.ListView
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.extract.Extract
import com.darakeon.dfm.api.old.DELETE
import com.darakeon.dfm.auth.auth
import com.darakeon.dfm.auth.highLightColor
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.dialogs.IYesNoDialogAnswer
import com.darakeon.dfm.dialogs.alertYesNo
import com.darakeon.dfm.dialogs.getDateDialog
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.createMove
import com.darakeon.dfm.extensions.formatNoDay
import com.darakeon.dfm.extensions.fromJson
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.extensions.setValueColored
import com.google.gson.Gson
import kotlinx.android.synthetic.main.extract.empty_list
import kotlinx.android.synthetic.main.extract.main_table
import kotlinx.android.synthetic.main.extract.reportChange
import kotlinx.android.synthetic.main.extract.total_title
import kotlinx.android.synthetic.main.extract.total_value
import java.util.Calendar

class ExtractActivity : BaseActivity<DELETE>(DELETE), IYesNoDialogAnswer {
	private val accountUrl: String get() = getExtraOrUrl("accountUrl")

	override val contentView = R.layout.extract
	override val contextMenuResource = R.menu.move_options
	override val viewWithContext: ListView get() = main_table

	private val clickedMove get() = clickedView as MoveLine

	private lateinit var extract: Extract
	private val extractKey = "extract"

	private val now = Calendar.getInstance()
	private var year: Int = now.get(Calendar.YEAR)
	private val yearKey = "year"
	private var month: Int = now.get(Calendar.MONTH)
	private val monthKey = "month"

	private val dialog: DatePickerDialog
		get() = getDateDialog(
			{ y, m, _ -> updateScreen(y, m) },
			year, month
		)

	private fun updateScreen(year: Int, month: Int) {
		setDate(month, year)
		getExtract()
	}

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		highlight?.setBackgroundColor(highLightColor)

		val extractJson = savedInstanceState?.get(extractKey)
		if (extractJson != null) {
			extract = extractJson.fromJson()

			setDate(
				savedInstanceState.getInt(monthKey),
				savedInstanceState.getInt(yearKey)
			)

			fillMoves()
		} else {
			setDateFromCaller()
			getExtract()
		}
	}

	private fun setDateFromCaller() {
		if (query.containsKey("id")) {
			val id = query["id"]?.toInt() ?: 0
			val startMonth = id.rem(100) - 1
			val startYear = id / 100
			setDate(startMonth, startYear)
		} else {
			val startMonth = intent.getIntExtra("month", month)
			val startYear = intent.getIntExtra("year", year)
			setDate(startMonth, startYear)
		}
	}

	private fun setDate(month: Int, year: Int) {
		this.month = month
		this.year = year

		intent.putExtra("month", month)
		intent.putExtra("year", year)

		val date = Calendar.getInstance()
		date.set(Calendar.MONTH, month)
		date.set(Calendar.YEAR, year)
		reportChange.text = date.formatNoDay()
	}

	override fun onSaveInstanceState(outState: Bundle) {
		super.onSaveInstanceState(outState)
		val json = Gson().toJson(extract)
		outState.putCharSequence(extractKey, json)
		outState.putInt(yearKey, year)
		outState.putInt(monthKey, month)
	}

	fun changeDate(@Suppress(ON_CLICK) view: View) {
		dialog.show()
	}

	private fun getExtract() {
		val time = year * 100 + month + 1

		api.getExtract(auth, accountUrl, time, this::handleMoves)
	}

	private fun handleMoves(data: Extract) {
		extract = data
		fillMoves()
	}

	private fun fillMoves() {
		total_title.text = extract.name
		setValueColored(total_value, extract.total)

		if (extract.moveList.isEmpty()) {
			main_table.visibility = View.GONE
			empty_list.visibility = View.VISIBLE
		} else {
			main_table.visibility = View.VISIBLE
			empty_list.visibility = View.GONE

			val accountAdapter = MoveAdapter(this, extract.moveList, extract.canCheck)
			main_table.adapter = accountAdapter
		}
	}


	fun goToSummary(@Suppress(ON_CLICK) view: View) {
		redirect<ExtractActivity> {
			it.putExtra("accountUrl", accountUrl)
			it.putExtra("year", year)
		}
	}

	private fun goToMove(moveId: Int) {
		val extras = Bundle()

		extras.putInt("id", moveId)
		extras.putString("accountUrl", accountUrl)
		extras.putInt("year", year)
		extras.putInt("month", month)

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
				api.check(auth, clickedMove.id, this::refresh)
				return true
			}
			R.id.uncheck_move -> {
				api.uncheck(auth, clickedMove.id, this::refresh)
				return true
			}
			else -> return super.onContextItemSelected(item)
		}
	}

	private fun edit() {
		goToMove(clickedMove.id)
	}


	private fun askDelete(): Boolean {
		var messageText = getString(R.string.sure_to_delete)

		messageText = String.format(messageText, clickedMove.description)

		alertYesNo(messageText, this)

		return false
	}

	override fun yesAction() {
		api.delete(auth, clickedMove.id, this::refresh)
	}

	override fun noAction() {}

	public override fun changeContextMenu(view: View, menuInfo: ContextMenu) {
		try {
			if (extract.canCheck) {
				hideMenuItem(menuInfo, if (clickedMove.isChecked) R.id.check_move else R.id.uncheck_move)
				showMenuItem(menuInfo, if (clickedMove.isChecked) R.id.uncheck_move else R.id.check_move)
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
