package com.darakeon.dfm.extract

import android.app.Dialog
import android.net.Uri
import android.os.Bundle
import android.os.PersistableBundle
import android.view.View
import android.widget.Button
import com.darakeon.dfm.R
import com.darakeon.dfm.databinding.ExtractBinding
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.Date
import com.darakeon.dfm.lib.api.entities.Environment
import com.darakeon.dfm.lib.api.entities.Theme
import com.darakeon.dfm.lib.api.entities.extract.Extract
import com.darakeon.dfm.lib.auth.setEnvironment
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.api.guid
import com.darakeon.dfm.testutils.api.readBundle
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.getDecimal
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.activity.getLastDatePicker
import com.darakeon.dfm.utils.api.ActivityMock
import com.google.gson.Gson
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.After
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import org.robolectric.shadows.ShadowAlertDialog.getLatestAlertDialog
import java.util.Calendar
import java.util.UUID

@RunWith(RobolectricTestRunner::class)
class ExtractActivityTest: BaseTest() {
	private lateinit var mocker: ActivityMock<ExtractActivity>
	private lateinit var activity: ExtractActivity

	private val currentYear = Calendar.getInstance()[Calendar.YEAR]
	private val currentMonth = Calendar.getInstance()[Calendar.MONTH]
	private val aMonth = if (currentMonth == 2) 1 else 3
	private val aMonthJava = aMonth - 1
	private val aMonthId = if (currentMonth == 2) "01" else "03"
	private val aMonthName = if (currentMonth == 2) "Jan" else "Mar"

	@Before
	fun setup() {
		mocker = ActivityMock(ExtractActivity::class)
		activity = mocker.get()
	}

	@After
	fun tearDown() {
		mocker.server.shutdown()
	}

	@Test
	fun structure() {
		activity.onCreate(Bundle(), null)

		assertNotNull(activity.findViewById(R.id.highlight))
		assertNotNull(activity.findViewById(R.id.total_title))
		assertNotNull(activity.findViewById(R.id.total_value))
		assertNotNull(activity.findViewById(R.id.report_change))
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
		activity.waitTasks(mocker.server)

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
		activity.waitTasks(mocker.server)

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
		activity.waitTasks(mocker.server)

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
		activity.waitTasks(mocker.server)

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
		activity.waitTasks(mocker.server)

		val binding = ExtractBinding.bind(
			shadowOf(activity).contentView
		)

		val extract = activity.getPrivate<Extract>("extract")
		assertThat(extract.title, `is`("account"))
		assertThat(extract.total, `is`(27.0))
		assertThat(extract.moveList.size, `is`(1))
		assertThat(extract.moveList[0].description, `is`("move"))
		assertThat(extract.moveList[0].date, `is`(Date(2020, 3, 8)))
		assertThat(extract.moveList[0].total, `is`(27.0))
		assertThat(extract.moveList[0].checked, `is`(true))
		assertThat(extract.moveList[0].guid, `is`(guid))

		assertThat(binding.mainTable.adapter.count, `is`(1))
	}

	@Test
	fun onCreateWithSavedState() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putInt("month", aMonthJava)
		saved.putString("extract", readBundle("extract_filled"))

		activity.onCreate(saved, null)

		val binding = ExtractBinding.bind(
			shadowOf(activity).contentView
		)

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
		assertThat(extract.moveList[0].guid, `is`(guid))

		assertThat(binding.reportChange.text.toString(), `is`("$aMonthName/1986"))
		assertThat(binding.mainTable.adapter.count, `is`(1))
	}

	@Test
	fun onCreateSetDate() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putInt("month", aMonthJava)

		activity.onCreate(saved, null)

		val binding = ExtractBinding.bind(
			shadowOf(activity).contentView
		)

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(1986))

		val month = activity.getPrivate<Int>("month")
		assertThat(month, `is`(aMonthJava))

		assertThat(
			binding.reportChange.text.toString(),
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

		val binding = ExtractBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.totalTitle.text.toString(), `is`("account"))
		assertThat(binding.totalValue.text.toString(), `is`("+27.00".getDecimal()))
		val color = activity.getColor(android.R.color.holo_blue_dark)
		assertThat(binding.totalValue.currentTextColor, `is`(color))
	}

	@Test
	fun onCreateFillExtractListEmpty() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putInt("month", aMonthJava)
		saved.putString("extract", readBundle("extract_empty"))

		activity.onCreate(saved, null)

		val binding = ExtractBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.emptyList.visibility, `is`(View.VISIBLE))
		assertThat(binding.mainTable.visibility, `is`(View.GONE))
		assertNull(binding.mainTable.adapter)
	}

	@Test
	fun onCreateFillExtractListFilled() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putString("extract", readBundle("extract_filled"))

		activity.onCreate(saved, null)

		val binding = ExtractBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.emptyList.visibility, `is`(View.GONE))
		assertThat(binding.mainTable.visibility, `is`(View.VISIBLE))
		assertThat(binding.mainTable.adapter.count, `is`(1))
	}

	@Test
	fun dateDialog() {
		val saved = Bundle()
		saved.putString("extract", readBundle("extract_empty"))
		activity.simulateNetwork()

		activity.onCreate(saved, null)

		val binding = ExtractBinding.bind(
			shadowOf(activity).contentView
		)

		assertNull(binding.mainTable.adapter)

		activity.changeDate(View(activity))

		mocker.server.enqueue("extract")

		val dialog = getLastDatePicker()
		dialog.updateDate(1986, aMonthJava, 1)
		dialog.getButton(Dialog.BUTTON_POSITIVE).performClick()
		activity.waitTasks(mocker.server)

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(1986))

		val month = activity.getPrivate<Int>("month")
		assertThat(month, `is`(aMonthJava))

		val extract = activity.getPrivate<Extract>("extract")
		assertThat(extract.moveList.size, `is`(1))
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
	fun goToSummary() {
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

		assertThat(intent.getCalledName(), `is`("SummaryActivity"))
		assertThat(intent.getIntExtra("year", 0), `is`(1986))
	}

	@Test
	fun onEdit() {
		val line = populateListAndOpenMenu()

		val edit = line.findViewById<Button>(R.id.action_edit)
		edit.performClick()

		val intent = shadowOf(activity).peekNextStartedActivity()
		assertThat(intent.getCalledName(), `is`("MovesActivity"))
		assertThat(intent.getSerializableExtra("id") as UUID?, `is`(guid))
		assertThat(intent.getStringExtra("accountUrl"), `is`("url"))
		assertThat(intent.getIntExtra("year", 0), `is`(1986))
		assertThat(intent.getIntExtra("month", 0), `is`(aMonthJava))

		val parent = intent.getSerializableExtra("__parent") as Class<*>
		assertThat(parent.simpleName, `is`("ExtractActivity"))
	}

	@Test
	fun onDelete() {
		val line = populateListAndOpenMenu()

		val delete = line.findViewById<Button>(R.id.action_delete)
		delete.performClick()

		mocker.server.enqueue("empty")
		val confirm = getLatestAlertDialog()
		confirm.getButton(Dialog.BUTTON_POSITIVE).performClick()
		activity.waitTasks(mocker.server)

		val intent = shadowOf(activity).peekNextStartedActivity()
		assertThat(intent.getCalledName(), `is`("ExtractActivity"))
	}

	@Test
	fun onCheck() {
		val line = populateListAndOpenMenu("extract_unchecked")

		val check = line.findViewById<Button>(R.id.action_check)
		val uncheck = line.findViewById<Button>(R.id.action_uncheck)

		assertThat(check.visibility, `is`(View.VISIBLE))
		assertThat(uncheck.visibility, `is`(View.GONE))

		mocker.server.enqueue("empty")
		check.performClick()
		activity.waitTasks(mocker.server)

		assertTrue(line.isChecked)

		assertThat(check.visibility, `is`(View.GONE))
		assertThat(uncheck.visibility, `is`(View.VISIBLE))
	}

	@Test
	fun onUncheck() {
		val line = populateListAndOpenMenu("extract_checked")
		line.performLongClick()

		val check = line.findViewById<Button>(R.id.action_check)
		val uncheck = line.findViewById<Button>(R.id.action_uncheck)

		assertThat(check.visibility, `is`(View.GONE))
		assertThat(uncheck.visibility, `is`(View.VISIBLE))

		mocker.server.enqueue("empty")
		uncheck.performClick()
		activity.waitTasks(mocker.server)

		assertFalse(line.isChecked)

		assertThat(check.visibility, `is`(View.VISIBLE))
		assertThat(uncheck.visibility, `is`(View.GONE))
	}

	@Test
	fun uncheckable() {
		val line = populateListAndOpenMenu("extract_uncheckable")

		val check = line.findViewById<Button>(R.id.action_check)
		assertThat(check.visibility, `is`(View.GONE))

		val uncheck = line.findViewById<Button>(R.id.action_uncheck)
		assertThat(uncheck.visibility, `is`(View.GONE))
	}

	private fun populateListAndOpenMenu(jsonName: String = "extract_filled"): MoveLine {
		activity.simulateNetwork()

		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putInt("month", aMonthJava)
		saved.putString("extract", readBundle(jsonName))
		activity.intent.putExtra("accountUrl", "url")

		activity.setEnvironment(Environment(Theme.DarkMagic))
		activity.onCreate(saved, null)

		val binding = ExtractBinding.bind(
			shadowOf(activity).contentView
		)

		shadowOf(binding.mainTable).populateItems()
		val line = binding.mainTable
			.getChildAt(0) as MoveLine
		line.performLongClick()

		return line
	}
}
