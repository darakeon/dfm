package com.darakeon.dfm.summary

import android.app.Dialog
import android.net.Uri
import android.os.Bundle
import android.os.PersistableBundle
import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.databinding.SummaryBinding
import com.darakeon.dfm.extensions.getFromJson
import com.darakeon.dfm.extensions.putJson
import com.darakeon.dfm.lib.api.entities.summary.Summary
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.api.readBundle
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
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf
import java.util.Calendar

@RunWith(RobolectricTestRunner::class)
class SummaryActivityTest: BaseTest() {
	private lateinit var mocker: ActivityMock<SummaryActivity>
	private lateinit var activity: SummaryActivity

	private val currentYear = Calendar.getInstance()[Calendar.YEAR]

	@Before
	fun setup() {
		mocker = ActivityMock(SummaryActivity::class)
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
		activity.waitTasks(mocker.server)

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
		activity.waitTasks(mocker.server)

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
		activity.waitTasks(mocker.server)

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
		activity.waitTasks(mocker.server)

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

		val binding = SummaryBinding.bind(
			shadowOf(activity).contentView
		)

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

		assertThat(binding.mainTable.adapter.count, `is`(2))
	}

	@Test
	fun onCreateWithSavedState() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putString("summary", readBundle("summary_filled"))

		activity.onCreate(saved, null)

		val binding = SummaryBinding.bind(
			shadowOf(activity).contentView
		)

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

		assertThat(binding.mainTable.adapter.count, `is`(2))
	}

	@Test
	fun onCreateSetDate() {
		val saved = Bundle()
		saved.putInt("year", 1986)

		activity.onCreate(saved, null)

		val binding = SummaryBinding.bind(
			shadowOf(activity).contentView
		)

		val year = activity.getPrivate<Int>("year")
		assertThat(year, `is`(1986))

		assertThat(
			binding.reportChange.text.toString(),
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

		val binding = SummaryBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.totalTitle.text.toString(), `is`("account"))
		assertThat(binding.totalValue.text.toString(), `is`("+2.00".getDecimal()))
		val color = activity.getColor(android.R.color.holo_blue_dark)
		assertThat(binding.totalValue.currentTextColor, `is`(color))
	}

	@Test
	fun onCreateFillSummaryListEmpty() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putString("summary", readBundle("summary_empty"))

		activity.onCreate(saved, null)

		val binding = SummaryBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.emptyList.visibility, `is`(View.VISIBLE))
		assertThat(binding.mainTable.visibility, `is`(View.GONE))
		assertNull(binding.mainTable.adapter)
	}

	@Test
	fun onCreateFillSummaryListFilled() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putString("summary", readBundle("summary_filled"))

		activity.onCreate(saved, null)

		val binding = SummaryBinding.bind(
			shadowOf(activity).contentView
		)

		assertThat(binding.emptyList.visibility, `is`(View.GONE))
		assertThat(binding.mainTable.visibility, `is`(View.VISIBLE))
		assertThat(binding.mainTable.adapter.count, `is`(2))
	}

	@Test
	fun dateDialog() {
		val saved = Bundle()
		saved.putInt("year", 1986)
		saved.putString("summary", readBundle("summary_empty"))
		activity.simulateNetwork()

		activity.onCreate(saved, null)

		val binding = SummaryBinding.bind(
			shadowOf(activity).contentView
		)

		assertNull(binding.mainTable.adapter)

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
