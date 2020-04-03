package com.darakeon.dfm.summary

import android.os.Bundle
import com.darakeon.dfm.R
import com.darakeon.dfm.utils.activity.ActivityMock
import com.darakeon.dfm.utils.activity.TestActivity
import com.darakeon.dfm.utils.activity.getActivityName
import com.darakeon.dfm.utils.getDecimal
import com.darakeon.dfm.utils.log.LogRule
import kotlinx.android.synthetic.main.month_line.view.name
import kotlinx.android.synthetic.main.month_line.view.value
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import org.robolectric.Shadows.shadowOf

@RunWith(RobolectricTestRunner::class)
class MonthLineTest {
	@get:Rule
	val log = LogRule()

	private lateinit var activity: TestActivity
	private lateinit var monthLine: MonthLine

	@Before
	fun setup() {
		activity = ActivityMock().create()
		monthLine = activity.layoutInflater
			.inflate(R.layout.month_line, null)
			as MonthLine
	}

	@Test
	fun setMonthPositive() {
		monthLine.setMonth("march", 27.0, "url", 1986, 3)

		assertThat(monthLine.name.text.toString(), `is`("march"))
		assertThat(monthLine.value.text.toString(), `is`("27.00".getDecimal()))

		val color = activity.getColor(R.color.positive_dark)
		assertThat(monthLine.value.currentTextColor, `is`(color))
	}

	@Test
	fun setMonthNegative() {
		monthLine.setMonth("march", -27.0, "url", 1986, 3)

		assertThat(monthLine.name.text.toString(), `is`("march"))
		assertThat(monthLine.value.text.toString(), `is`("27.00".getDecimal()))

		val color = activity.getColor(R.color.negative_dark)
		assertThat(monthLine.value.currentTextColor, `is`(color))
	}

	@Test
	fun setMonthClick() {
		monthLine.setMonth("march", -27.0, "url", 1986, 3)
		monthLine.performClick()

		val intent = shadowOf(activity)
			.peekNextStartedActivity()

		val extras = intent?.extras ?: Bundle()
		assertThat(extras.getString("accountUrl"), `is`("url"))
		assertThat(extras.getInt("year"), `is`(1986))
		assertThat(extras.getInt("month"), `is`(2))

		val activity = intent.getActivityName()
		assertThat(activity, `is`("ExtractActivity"))
	}
}
