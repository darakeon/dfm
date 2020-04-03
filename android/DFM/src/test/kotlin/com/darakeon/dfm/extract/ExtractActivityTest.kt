package com.darakeon.dfm.extract

import android.app.DatePickerDialog
import android.app.Dialog
import android.net.Uri
import android.os.Bundle
import android.os.PersistableBundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Date
import com.darakeon.dfm.api.entities.Environment
import com.darakeon.dfm.api.entities.extract.Extract
import com.darakeon.dfm.auth.setEnvironment
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.getPrivate
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.activity.getActivityName
import com.darakeon.dfm.utils.api.readBundle
import com.darakeon.dfm.utils.getDecimal
import com.darakeon.dfm.utils.log.LogRule
import com.darakeon.dfm.utils.robolectric.simulateNetwork
import com.google.gson.Gson
import kotlinx.android.synthetic.main.extract.empty_list
import kotlinx.android.synthetic.main.extract.main_table
import kotlinx.android.synthetic.main.extract.reportChange
import kotlinx.android.synthetic.main.extract.total_title
import kotlinx.android.synthetic.main.extract.total_value
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog
import org.robolectric.shadows.ShadowDialog.getLatestDialog
import java.util.Calendar

@RunWith(RobolectricTestRunner::class)
class ExtractActivityTest {
	@get:Rule
	val log = LogRule()

	private lateinit var mocker: ActivityMock
	private lateinit var activity: ExtractActivity

	private val currentYear = Calendar.getInstance()[Calendar.YEAR]
	private val currentMonth = Calendar.getInstance()[Calendar.MONTH]
	private val aMonth = if (currentMonth == 2) 1 else 3
	private val aMonthJava = aMonth - 1
	private val aMonthId = if (currentMonth == 2) "01" else "03"
	private val aMonthName = if (currentMonth == 2) "Jan" else "Mar"

	@Before
	fun setup() {
		mocker = ActivityMock()
		activity = mocker.get<ExtractActivity>()
	}

	@Test
	fun structure() {
		activity.onCreate(null, null)
		assertNotNull(activity.findViewById(R.id.highlight))
		assertNotNull(activity.findViewById(R.id.total_title))
		assertNotNull(activity.findViewById(R.id.total_value))
		assertNotNull(activity.findViewById(R.id.reportChange))
		assertNotNull(activity.findViewById(R.id.action_summary))
		assertNotNull(activity.findViewById(R.id.empty_list))
		assertNotNull(activity.findViewById(R.id.main_table))
	}

	@Test
	fun onCreateFromApiByIntent() {
		activity.intent.putExtra("accountUrl", "url")
		activity.intent.putExtra("year", 1986)
		activity.intent.putExtra("month", aMonthJava)
		activity.simulateNetwork()
		mocker.server.enqueue("extract")

		activity.onCreate(null, null)

		val accountUrl = activity.getPrivate<String>("accountUrl")
		assertThat(accountUrl, `is`("url"))

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(1986))

		val month = activity.getPrivate<Int>("month")
		assertThat(month, `is`(aMonthJava))
	}

	@Test
	fun onCreateFromApiByQuery() {
		activity.intent.data = Uri.parse("?id=1986$aMonthId&accountUrl=url")
		activity.simulateNetwork()
		mocker.server.enqueue("extract")

		activity.onCreate(null, null)

		val accountUrl = activity.getPrivate<String>("accountUrl")
		assertThat(accountUrl, `is`("url"))

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(1986))

		val month = activity.getPrivate<Int>("month")
		assertThat(month, `is`(aMonthJava))
	}

	@Test
	fun onCreateFromApiByQueryInvalidID() {
		activity.intent.data = Uri.parse("?id=X&accountUrl=url")
		activity.simulateNetwork()
		mocker.server.enqueue("extract")

		activity.onCreate(null, null)

		val accountUrl = activity.getPrivate<String>("accountUrl")
		assertThat(accountUrl, `is`("url"))

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(currentYear))

		val month = activity.getPrivate<Int>("month")
		assertThat(month, `is`(currentMonth))
	}

	@Test
	fun onCreateFromApiDateEmpty() {
		activity.intent.putExtra("accountUrl", "url")
		activity.simulateNetwork()
		mocker.server.enqueue("extract")

		activity.onCreate(null, null)

		val accountUrl = activity.getPrivate<String>("accountUrl")
		assertThat(accountUrl, `is`("url"))

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(currentYear))

		val month = activity.getPrivate<Int>("month")
		assertThat(month, `is`(currentMonth))
	}

	@Test
	fun onCreateFromApiExtract() {
		activity.intent.putExtra("accountUrl", "url")
		activity.simulateNetwork()
		mocker.server.enqueue("extract")

		activity.onCreate(null, null)

		val extract = activity.getPrivate<Extract>("extract")
		assertThat(extract.title, `is`("account"))
		assertThat(extract.total, `is`(27.0))
		assertThat(extract.moveList.size, `is`(1))
		assertThat(extract.moveList[0].description, `is`("move"))
		assertThat(extract.moveList[0].date, `is`(Date(2020, 3, 8)))
		assertThat(extract.moveList[0].total, `is`(27.0))
		assertThat(extract.moveList[0].checked, `is`(true))
		assertThat(extract.moveList[0].id, `is`(1))

		assertThat(activity.main_table.adapter.count, `is`(1))
	}

	@Test
	fun onCreateWithSavedState() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putInt("month", aMonthJava)
		saved.putString("extract", readBundle("extract_filled"))

		activity.onCreate(saved, null)

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(1986))

		val month = activity.getPrivate<Int>("month")
		assertThat(month, `is`(aMonthJava))

		val extract = activity.getPrivate<Extract>("extract")
		assertThat(extract.title, `is`("account"))
		assertThat(extract.total, `is`(27.0))
		assertThat(extract.moveList.size, `is`(1))
		assertThat(extract.moveList[0].description, `is`("move"))
		assertThat(extract.moveList[0].date, `is`(Date(2020, 3, 8)))
		assertThat(extract.moveList[0].total, `is`(27.0))
		assertThat(extract.moveList[0].checked, `is`(true))
		assertThat(extract.moveList[0].id, `is`(1))

		assertThat(activity.reportChange.text.toString(), `is`("$aMonthName/1986"))
		assertThat(activity.main_table.adapter.count, `is`(1))
	}

	@Test
	fun onCreateSetDate() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putInt("month", aMonthJava)

		activity.onCreate(saved, null)

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(1986))

		val month = activity.getPrivate<Int>("month")
		assertThat(month, `is`(aMonthJava))

		assertThat(
			activity.reportChange.text.toString(),
			`is`("$aMonthName/1986")
		)

		assertThat(
			activity.intent.getIntExtra("year", 0),
			`is`(1986)
		)

		assertThat(
			activity.intent.getIntExtra("month", 0),
			`is`(aMonthJava)
		)
	}

	@Test
	fun onCreateFillExtractHeader() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putInt("month", aMonthJava)
		saved.putString("extract", readBundle("extract_filled"))

		activity.onCreate(saved, null)

		assertThat(activity.total_title.text.toString(), `is`("account"))
		assertThat(activity.total_value.text.toString(), `is`("27.00".getDecimal()))
		val color = activity.getColor(R.color.positive_dark)
		assertThat(activity.total_value.currentTextColor, `is`(color))
	}

	@Test
	fun onCreateFillExtractListEmpty() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putInt("month", aMonthJava)
		saved.putString("extract", readBundle("extract_empty"))

		activity.onCreate(saved, null)

		assertThat(activity.empty_list.visibility, `is`(View.VISIBLE))
		assertThat(activity.main_table.visibility, `is`(View.GONE))
		assertNull(activity.main_table.adapter)
	}

	@Test
	fun onCreateFillExtractListFilled() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putString("extract", readBundle("extract_filled"))

		activity.onCreate(saved, null)

		assertThat(activity.empty_list.visibility, `is`(View.GONE))
		assertThat(activity.main_table.visibility, `is`(View.VISIBLE))
		assertThat(activity.main_table.adapter.count, `is`(1))
	}

	@Test
	fun dateDialog() {
		val saved = Bundle()
		saved.putString("extract", readBundle("extract_empty"))
		activity.simulateNetwork()

		activity.onCreate(saved, null)

		assertNull(activity.main_table.adapter)

		activity.changeDate(View(activity))

		val dialog = getLatestDialog() as DatePickerDialog
		assertNotNull(dialog)

		mocker.server.enqueue("extract")

		dialog.updateDate(1986, aMonthJava, 1)
		dialog.getButton(Dialog.BUTTON_POSITIVE).performClick()

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(1986))

		val month = activity.getPrivate<Int>("month")
		assertThat(month, `is`(aMonthJava))

		assertThat(activity.main_table.adapter.count, `is`(1))
	}

	@Test
	fun onSaveInstance() {
		val originalYear = 1986
		val originalMonth = aMonth
		val originalExtract = Gson().fromJson(
			readBundle("extract_filled"),
			Extract::class.java
		)

		val originalState = Bundle()
		originalState.putInt("year", originalYear)
		originalState.putInt("month", originalMonth)
		originalState.putJson("extract", originalExtract)

		activity.onCreate(originalState, null)

		val newState = Bundle()
		activity.onSaveInstanceState(newState, PersistableBundle())

		val newYear = newState.getInt("year", 0)
		assertThat(newYear, `is`(originalYear))

		val newMonth = newState.getInt("month", 0)
		assertThat(newMonth, `is`(originalMonth))

		val newExtract = newState.getFromJson("extract", Extract())
		assertThat(newExtract, `is`(originalExtract))
	}

	@Test
	fun goToAccounts() {
		activity.simulateNetwork()

		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putInt("month", aMonthJava)
		saved.putString("extract", readBundle("extract_empty"))
		activity.onCreate(saved, null)

		val view = View(activity)
		activity.goToSummary(view)

		val intent = shadowOf(activity)
			.peekNextStartedActivity()

		assertThat(intent.getActivityName(), `is`("SummaryActivity"))
		assertThat(intent.getIntExtra("year", 0), `is`(1986))
	}

	@Test
	fun onContextItemSelectedEdit() {
		val line = populateListAndOpenMenu()

		val item = line.menu!!.getItem(0)
		activity.onContextItemSelected(item)

		val intent = shadowOf(activity).peekNextStartedActivity()
		assertThat(intent.getActivityName(), `is`("MovesActivity"))
		assertThat(intent.getIntExtra("id", 0), `is`(1))
		assertThat(intent.getStringExtra("accountUrl"), `is`("url"))
		assertThat(intent.getIntExtra("year", 0), `is`(1986))
		assertThat(intent.getIntExtra("month", 0), `is`(aMonthJava))

		val parent = intent.getSerializableExtra("__parent") as Class<*>
		assertThat(parent.simpleName, `is`("ExtractActivity"))
	}

	@Test
	fun onContextItemSelectedDelete() {
		val line = populateListAndOpenMenu()

		val item = line.menu!!.getItem(1)
		activity.onContextItemSelected(item)

		mocker.server.enqueue("empty")
		val confirm = getLatestAlertDialog()
		confirm.getButton(Dialog.BUTTON_POSITIVE).performClick()

		val intent = shadowOf(activity).peekNextStartedActivity()
		assertThat(intent.getActivityName(), `is`("ExtractActivity"))
	}

	@Test
	fun onContextItemSelectedCheck() {
		val line = populateListAndOpenMenu("extract_unchecked")

		val hidden = line.menu!!.getItem(3)
		assertFalse(hidden.isVisible)

		mocker.server.enqueue("empty")

		val item = line.menu!!.getItem(2)
		activity.onContextItemSelected(item)

		assertTrue(line.isChecked)
	}

	@Test
	fun onContextItemSelectedUncheck() {
		val line = populateListAndOpenMenu("extract_checked")

		val hidden = line.menu!!.getItem(2)
		assertFalse(hidden.isVisible)

		mocker.server.enqueue("empty")

		val item = line.menu!!.getItem(3)
		activity.onContextItemSelected(item)

		assertFalse(line.isChecked)
	}

	@Test
	fun onContextItemSelectedUncheckable() {
		val line = populateListAndOpenMenu("extract_uncheckable")

		val check = line.menu!!.getItem(2)
		assertFalse(check.isVisible)

		val uncheck = line.menu!!.getItem(3)
		assertFalse(uncheck.isVisible)
	}

	private fun populateListAndOpenMenu(jsonName: String = "extract_filled"): MoveLine {
		activity.simulateNetwork()

		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putInt("month", aMonthJava)
		saved.putString("extract", readBundle(jsonName))
		activity.intent.putExtra("accountUrl", "url")

		activity.setEnvironment(Environment("Dark", ""))
		activity.onCreate(saved, null)

		shadowOf(activity.main_table).populateItems()
		val line = activity.main_table
			.getChildAt(0) as MoveLine
		line.performClick()

		return line
	}
}
