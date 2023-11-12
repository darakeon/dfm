package com.darakeon.dfm.summary

import android.os.Bundle
import com.darakeon.dfm.R
import com.darakeon.dfm.databinding.MonthLineBinding
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.context.getCalledName
import com.darakeon.dfm.testutils.getDecimal
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf

@RunWith(RobolectricTestRunner::class)
class MonthLineTest: BaseTest() {
	private lateinit var activity: SummaryActivity
	private lateinit var monthLine: MonthLine
	private lateinit var binding: MonthLineBinding

	@Before
	fun setup() {
		activity = ActivityMock(SummaryActivity::class).get()
		activity.onCreate(Bundle(), null)

		monthLine = activity.layoutInflater
			.inflate(R.layout.month_line, null)
			as MonthLine

		binding = MonthLineBinding.bind(monthLine)
	}

	@Test
	fun setMonthPositive() {
		monthLine.setMonth("march", 27.0, "url", 1986, 3)

		assertThat(binding.name.text.toString(), `is`("march"))
		assertThat(binding.value.text.toString(), `is`("+27.00".getDecimal()))

		val color = activity.getColor(android.R.color.holo_blue_dark)
		assertThat(binding.value.currentTextColor, `is`(color))
	}

	@Test
	fun setMonthNegative() {
		monthLine.setMonth("march", -27.0, "url", 1986, 3)

		assertThat(binding.name.text.toString(), `is`("march"))
		assertThat(binding.value.text.toString(), `is`("-27.00".getDecimal()))

		val color = activity.getColor(android.R.color.holo_red_dark)
		assertThat(binding.value.currentTextColor, `is`(color))
	}

	@Test
	fun setMonthClick() {
		monthLine.setMonth("march", -27.0, "url", 1986, 3)
		monthLine.performLongClick()

		val intent = shadowOf(activity)
			.peekNextStartedActivity()

		val extras = intent?.extras ?: Bundle()
		assertThat(extras.getString("accountUrl"), `is`("url"))
		assertThat(extras.getInt("year"), `is`(1986))
		assertThat(extras.getInt("month"), `is`(2))

		val activity = intent.getCalledName()
		assertThat(activity, `is`("ExtractActivity"))
	}
}
