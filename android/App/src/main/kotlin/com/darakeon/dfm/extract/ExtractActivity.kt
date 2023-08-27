package com.darakeon.dfm.extract

import android.os.Bundle
import android.view.View
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.ExtractBinding
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

class ExtractActivity : BaseActivity<ExtractBinding>() {
	private var accountUrl: String = ""

	private val now = Calendar.getInstance()
	private var year: Int = now.get(Calendar.YEAR)
	private val yearKey = "year"
	private var month: Int = now.get(Calendar.MONTH)
	private val monthKey = "month"

	private var extract = Extract()
	private val extractKey = "extract"

	override val contentViewId = R.layout.extract
	override val title = R.string.title_activity_extract

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

		if (newMonth < Calendar.JANUARY) {
			newMonth = Calendar.DECEMBER
			newYear -= 1
		}

		if (newMonth > Calendar.DECEMBER) {
			newMonth = Calendar.JANUARY
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

			main.listChild = main_table
			main.swipe(Direction.Right, this::past)
			main.swipe(Direction.Left, this::future)
		} else {
			main_table.visibility = View.VISIBLE
			empty_list.visibility = View.GONE

			main_table.adapter = MoveAdapter(
				this,
				extract.moveList,
				extract.canCheck,
				this::goToMove,
				this::askDelete,
				this::checkMove,
				this::uncheckMove
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

	private fun goToMove(move: MoveLine) {
		val extras = Bundle()

		extras.putSerializable("id", move.guid)
		extras.putString("accountUrl", accountUrl)
		extras.putInt("year", year)
		extras.putInt("month", month)

		createMove(extras)
	}

	private fun askDelete(move: MoveLine): Boolean {
		var messageText = getString(R.string.sure_to_delete)

		messageText = String.format(messageText, move.description)

		confirm(messageText) {
			callApi { it.delete(move.guid, this::refresh) }
		}

		return false
	}

	private fun checkMove(move: MoveLine) {
		callApi {
			it.check(move.guid, move.nature) {
				move.check()
			}
		}
	}

	private fun uncheckMove(move: MoveLine) {
		callApi {
			it.uncheck(move.guid, move.nature) {
				move.uncheck()
			}
		}
	}
}
