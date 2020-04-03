package com.darakeon.dfm.summary

import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.summary.Month
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.log.LogRule
import kotlinx.android.synthetic.main.month_line.view.name
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class MonthAdapterTest {
	@get:Rule
	val log = LogRule()

	@Test
	fun populateView() {
		val list = listOf(Month("september", 9, 0.0))
		val activity = ActivityMock().create<SummaryActivity>()
		val adapter = MonthAdapter(activity, list, "url", 1989)
		val line = activity.layoutInflater
			.inflate(R.layout.month_line, null)
			as MonthLine

		adapter.populateView(line, 0)

		assertThat(line.name.text.toString(), `is`("september"))
	}
}
