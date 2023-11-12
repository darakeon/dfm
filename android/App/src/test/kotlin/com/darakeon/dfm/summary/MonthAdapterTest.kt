package com.darakeon.dfm.summary

import com.darakeon.dfm.R
import com.darakeon.dfm.databinding.MonthLineBinding
import com.darakeon.dfm.lib.api.entities.summary.Month
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class MonthAdapterTest: BaseTest() {
	@Test
	fun populateView() {
		val list = listOf(Month("september", 9, 0.0))
		val activity = ActivityMock(SummaryActivity::class).create()
		val adapter = MonthAdapter(activity, list, "url", 1989)
		val line = activity.layoutInflater
			.inflate(R.layout.month_line, null)
			as MonthLine

		adapter.populateView(line, 0)

		val binding = MonthLineBinding.bind(line)

		assertThat(binding.name.text.toString(), `is`("september"))
	}
}
