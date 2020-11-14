package com.darakeon.dfm.summary

import android.app.Dialog
import android.net.Uri
import android.os.Bundle
import android.os.PersistableBundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.summary.Summary
import com.darakeon.dfm.testutils.LogRule
import com.darakeon.dfm.testutils.api.readBundle
import com.darakeon.dfm.testutils.getDecimal
import com.darakeon.dfm.testutils.getPrivate
import com.darakeon.dfm.testutils.robolectric.simulateNetwork
import com.darakeon.dfm.testutils.robolectric.waitTasks
import com.darakeon.dfm.utils.activity.getLastDatePicker
import com.darakeon.dfm.utils.api.ActivityMock
import com.google.gson.Gson
import kotlinx.android.synthetic.main.summary.empty_list
import kotlinx.android.synthetic.main.summary.main_table
import kotlinx.android.synthetic.main.summary.reportChange
import kotlinx.android.synthetic.main.summary.total_title
import kotlinx.android.synthetic.main.summary.total_value
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import java.util.Calendar

@RunWith(RobolectricTestRunner::class)
class SummaryActivityTest {
	@get:Rule
	val log = LogRule()

	private lateinit var mocker: ActivityMock<SummaryActivity>
	private lateinit var activity: SummaryActivity

	private val currentYear = Calendar.getInstance()[Calendar.YEAR]

	@Before
	fun setup() {
		mocker = ActivityMock(SummaryActivity::class)
		activity = mocker.get()
	}

	@Test
	fun structure() {
		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

		assertNotNull(activity.findViewById(R.id.highlight))
		assertNotNull(activity.findViewById(R.id.total_title))
		assertNotNull(activity.findViewById(R.id.total_value))
		assertNotNull(activity.findViewById(R.id.reportChange))
		assertNotNull(activity.findViewById(R.id.empty_list))
		assertNotNull(activity.findViewById(R.id.main_table))
	}

	@Test
	fun onCreateFromApiByIntent() {
		activity.intent.putExtra("accountUrl", "url")
		activity.intent.putExtra("year", 1986)
		activity.simulateNetwork()
		mocker.server.enqueue("summary")

		activity.onCreate(null, null)

		val accountUrl = activity.getPrivate<String>("accountUrl")
		assertThat(accountUrl, `is`("url"))

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(1986))
	}

	@Test
	fun onCreateFromApiByQuery() {
		activity.intent.data = Uri.parse("?id=1986&accountUrl=url")
		activity.simulateNetwork()
		mocker.server.enqueue("summary")

		activity.onCreate(null, null)

		val accountUrl = activity.getPrivate<String>("accountUrl")
		assertThat(accountUrl, `is`("url"))

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(1986))
	}

	@Test
	fun onCreateFromApiByQueryInvalidID() {
		activity.intent.data = Uri.parse("?id=X&accountUrl=url")
		activity.simulateNetwork()
		mocker.server.enqueue("summary")

		activity.onCreate(null, null)

		val accountUrl = activity.getPrivate<String>("accountUrl")
		assertThat(accountUrl, `is`("url"))

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(currentYear))
	}

	@Test
	fun onCreateFromApiDateEmpty() {
		activity.intent.putExtra("accountUrl", "url")
		activity.simulateNetwork()
		mocker.server.enqueue("summary")

		activity.onCreate(null, null)

		val accountUrl = activity.getPrivate<String>("accountUrl")
		assertThat(accountUrl, `is`("url"))

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(currentYear))
	}

	@Test
	fun onCreateFromApiSummary() {
		activity.intent.putExtra("accountUrl", "url")
		activity.simulateNetwork()
		mocker.server.enqueue("summary")

		activity.onCreate(null, null)
		activity.waitTasks(mocker.server)

		val summary = activity.getPrivate<Summary>("summary")
		assertThat(summary.title, `is`("account"))
		assertThat(summary.total, `is`(2.0))
		assertThat(summary.monthList.size, `is`(2))
		assertThat(summary.monthList[0].name, `is`("March"))
		assertThat(summary.monthList[0].number, `is`(3))
		assertThat(summary.monthList[0].total, `is`(1.0))
		assertThat(summary.monthList[1].name, `is`("December"))
		assertThat(summary.monthList[1].number, `is`(12))
		assertThat(summary.monthList[1].total, `is`(1.0))

		assertThat(activity.main_table.adapter.count, `is`(2))
	}

	@Test
	fun onCreateWithSavedState() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putString("summary", readBundle("summary_filled"))

		activity.onCreate(saved, null)

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(1986))

		val summary = activity.getPrivate<Summary>("summary")
		assertThat(summary.title, `is`("account"))
		assertThat(summary.total, `is`(2.0))
		assertThat(summary.monthList.size, `is`(2))
		assertThat(summary.monthList[0].name, `is`("March"))
		assertThat(summary.monthList[0].number, `is`(3))
		assertThat(summary.monthList[0].total, `is`(1.0))
		assertThat(summary.monthList[1].name, `is`("December"))
		assertThat(summary.monthList[1].number, `is`(12))
		assertThat(summary.monthList[1].total, `is`(1.0))

		assertThat(activity.main_table.adapter.count, `is`(2))
	}

	@Test
	fun onCreateSetDate() {
		val saved = Bundle()
		saved.putInt("year", 1986)

		activity.onCreate(saved, null)

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(1986))

		assertThat(
			activity.reportChange.text.toString(),
			`is`("1986")
		)

		assertThat(
			activity.intent.getIntExtra("year", 0),
			`is`(1986)
		)
	}

	@Test
	fun onCreateFillSummaryHeader() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putString("summary", readBundle("summary_filled"))

		activity.onCreate(saved, null)

		assertThat(activity.total_title.text.toString(), `is`("account"))
		assertThat(activity.total_value.text.toString(), `is`("+2.00".getDecimal()))
		val color = activity.getColor(R.color.positive_dark)
		assertThat(activity.total_value.currentTextColor, `is`(color))
	}

	@Test
	fun onCreateFillSummaryListEmpty() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putString("summary", readBundle("summary_empty"))

		activity.onCreate(saved, null)

		assertThat(activity.empty_list.visibility, `is`(View.VISIBLE))
		assertThat(activity.main_table.visibility, `is`(View.GONE))
		assertNull(activity.main_table.adapter)
	}

	@Test
	fun onCreateFillSummaryListFilled() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putString("summary", readBundle("summary_filled"))

		activity.onCreate(saved, null)

		assertThat(activity.empty_list.visibility, `is`(View.GONE))
		assertThat(activity.main_table.visibility, `is`(View.VISIBLE))
		assertThat(activity.main_table.adapter.count, `is`(2))
	}

	@Test
	fun dateDialog() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putString("summary", readBundle("summary_empty"))
		activity.simulateNetwork()

		activity.onCreate(saved, null)

		assertNull(activity.main_table.adapter)

		activity.changeDate(View(activity))

		mocker.server.enqueue("summary")

		val dialog = getLastDatePicker()
		dialog.updateDate(1986, 1, 1)
		dialog.getButton(Dialog.BUTTON_POSITIVE).performClick()
		activity.waitTasks(mocker.server)

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(1986))

		val summary = activity.getPrivate<Summary>("summary")
		assertThat(summary.monthList.size, `is`(2))
	}

	@Test
	fun onSaveInstance() {
		val originalYear = 1986
		val originalSummary = Gson().fromJson(
			readBundle("summary_filled"),
			Summary::class.java
		)

		val originalState = Bundle()
		originalState.putInt("year", originalYear)
		originalState.putJson("summary", originalSummary)

		activity.onCreate(originalState, null)

		val newState = Bundle()
		activity.onSaveInstanceState(newState, PersistableBundle())

		val newYear = newState.getInt("year", 0)
		assertThat(newYear, `is`(originalYear))

		val newSummary = newState.getFromJson("summary", Summary())
		assertThat(newSummary, `is`(originalSummary))
	}
}
