package com.darakeon.dfm.extract

import android.os.Bundle
import android.view.ContextMenu
import android.view.Menu
import android.view.MenuItem
import android.view.View
import android.widget.AdapterView
import android.widget.ListView
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.dialogs.confirm
import com.darakeon.dfm.dialogs.getDateDialog
import com.darakeon.dfm.extensions.ON_CLICK
import com.darakeon.dfm.extensions.createMove
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.extract.Extract
import com.darakeon.dfm.lib.extensions.Direction
import com.darakeon.dfm.lib.extensions.formatNoDay
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.lib.extensions.refresh
import com.darakeon.dfm.lib.extensions.setValueColored
import com.darakeon.dfm.lib.extensions.swipe
import com.darakeon.dfm.summary.SummaryActivity
import kotlinx.android.synthetic.main.extract.empty_list
import kotlinx.android.synthetic.main.extract.main
import kotlinx.android.synthetic.main.extract.main_table
import kotlinx.android.synthetic.main.extract.reportChange
import kotlinx.android.synthetic.main.extract.total_title
import kotlinx.android.synthetic.main.extract.total_value
import java.util.Calendar
import java.util.UUID

class ExtractActivity : BaseActivity() {
	private var accountUrl: String = ""

	private val now = Calendar.getInstance()
	private var year: Int = now.get(Calendar.YEAR)
	private val yearKey = "year"
	private var month: Int = now.get(Calendar.MONTH)
	private val monthKey = "month"

	private var extract = Extract()
	private val extractKey = "extract"

	override val contentView = R.layout.extract
	override val title = R.string.title_activity_extract

	override val contextMenuResource = R.menu.move_options
	override val viewWithContext: ListView get() = main_table

	override val refresh: SwipeRefreshLayout?
		get() = main

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)

		accountUrl = getExtraOrUrl("accountUrl") ?: ""

		if (savedInstanceState != null) {
			extract = savedInstanceState
				.getFromJson(extractKey, Extract())

			setDate(
				savedInstanceState.getInt(yearKey),
				savedInstanceState.getInt(monthKey)
			)

			fillMoves()
		} else {
			setDateFromCaller()
			getExtract()
		}
	}

	private fun setDateFromCaller() {
		val id = query["id"]?.toIntOrNull()

		if (id != null) {
			val startMonth = id.rem(100) - 1
			val startYear = id / 100
			setDate(startYear, startMonth)
		} else {
			val startYear = intent.getIntExtra("year", year)
			val startMonth = intent.getIntExtra("month", month)
			setDate(startYear, startMonth)
		}
	}

	private fun setDate(year: Int, month: Int) {
		this.year = year
		this.month = month

		intent.putExtra("year", year)
		intent.putExtra("month", month)

		val date = Calendar.getInstance()
		date.set(Calendar.YEAR, year)
		date.set(Calendar.MONTH, month)
		reportChange.text = date.formatNoDay()
	}

	override fun onSaveInstanceState(outState: Bundle) {
		super.onSaveInstanceState(outState)
		outState.putJson(extractKey, extract)
		outState.putInt(yearKey, year)
		outState.putInt(monthKey, month)
	}

	private fun past() {
		getExtract(year, month-1)
	}

	private fun future() {
		getExtract(year, month+1)
	}

	fun changeDate(@Suppress(ON_CLICK) view: View) {
		when (view.id) {
			R.id.prev -> past()
			R.id.next -> future()
			else ->
				getDateDialog(
					year, month, this::getExtract
				).show()
		}
	}

	private fun getExtract(y: Int, m: Int) {
		var newMonth = m
		var newYear = y

		if (newMonth == 0) {
			newMonth = 12
			newYear -= 1
		}

		if (newMonth == 13) {
			newMonth = 1
			newYear += 1
		}

		setDate(newYear, newMonth)
		getExtract()
	}

	private fun getExtract() {
		callApi {
			it.getExtract(accountUrl, year, month, this::handleMoves)
		}
	}

	private fun handleMoves(data: Extract) {
		extract = data
		fillMoves()
	}

	private fun fillMoves() {
		total_title.text = extract.title
		total_value.setValueColored(extract.total)

		if (extract.moveList.isEmpty()) {
			main_table.visibility = View.GONE
			empty_list.visibility = View.VISIBLE

			main.swipe(Direction.Right, this::past)
			main.swipe(Direction.Left, this::future)
		} else {
			main_table.visibility = View.VISIBLE
			empty_list.visibility = View.GONE

			main_table.adapter = MoveAdapter(
				this,
				extract.moveList,
				extract.canCheck
			)

			main_table.swipe(Direction.Right, this::past)
			main_table.swipe(Direction.Left, this::future)
		}
	}

	fun goToSummary(@Suppress(ON_CLICK) view: View) {
		redirect<SummaryActivity> {
			it.putExtra("accountUrl", accountUrl)
			it.putExtra("year", year)
		}
	}

	override fun onContextItemSelected(item: MenuItem): Boolean {
		val menuInfo = item.menuInfo as AdapterView.AdapterContextMenuInfo
		val move = menuInfo.targetView as MoveLine

		when (item.itemId) {
			R.id.edit_move -> {
				goToMove(move.guid)
				return true
			}
			R.id.delete_move -> {
				askDelete(move.guid, move.description)
				return true
			}
			R.id.check_move -> {
				callApi {
					it.check(move.guid, move.nature) {
						this.check(move)
					}
				}
				return true
			}
			R.id.uncheck_move -> {
				callApi {
					it.uncheck(move.guid, move.nature) {
						this.uncheck(move)
					}
				}
				return true
			}
			else -> return super.onContextItemSelected(item)
		}
	}

	private fun goToMove(moveId: UUID) {
		val extras = Bundle()

		extras.putSerializable("id", moveId)
		extras.putString("accountUrl", accountUrl)
		extras.putInt("year", year)
		extras.putInt("month", month)

		createMove(extras)
	}

	private fun askDelete(id: UUID, description: String): Boolean {
		var messageText = getString(R.string.sure_to_delete)

		messageText = String.format(messageText, description)

		confirm(messageText) {
			callApi { it.delete(id, this::refresh) }
		}

		return false
	}

	private fun check(move: MoveLine) {
		move.check()
		val menu = move.menu ?: return
		showUncheck(menu)
	}

	private fun uncheck(move: MoveLine) {
		move.uncheck()
		val menu = move.menu ?: return
		showCheck(menu)
	}

	public override fun changeContextMenu(
		menu: ContextMenu,
		view: View,
		menuInfo: ContextMenu.ContextMenuInfo
	) {
		val menuAdapter = menuInfo as
			AdapterView.AdapterContextMenuInfo

		val move = menuAdapter.targetView as MoveLine

		move.menu = menu

		if (extract.canCheck) {
			if (move.isChecked) {
				showUncheck(menu)
			} else {
				showCheck(menu)
			}
		} else {
			hideMenuItem(menu, R.id.check_move)
			hideMenuItem(menu, R.id.uncheck_move)
		}
	}

	private fun showCheck(menu: Menu) {
		showMenuItem(menu, R.id.check_move)
		hideMenuItem(menu, R.id.uncheck_move)
	}

	private fun showUncheck(menu: Menu) {
		showMenuItem(menu, R.id.uncheck_move)
		hideMenuItem(menu, R.id.check_move)
	}

	private fun hideMenuItem(menu: Menu, id: Int) {
		menu.findItem(id).isVisible = false
	}

	private fun showMenuItem(menu: Menu, id: Int) {
		menu.findItem(id).isVisible = true
	}
}
