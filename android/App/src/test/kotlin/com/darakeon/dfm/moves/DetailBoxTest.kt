package com.darakeon.dfm.moves

import android.widget.Button
import android.widget.LinearLayout
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.lib.api.entities.moves.Detail
import com.darakeon.dfm.lib.api.entities.moves.Move
import com.darakeon.dfm.testutils.BaseTest
import com.darakeon.dfm.testutils.getDecimal
import com.darakeon.dfm.utils.api.ActivityMock
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class DetailBoxTest: BaseTest() {
	private lateinit var activity: MovesActivity
	private lateinit var box: DetailBox
	private var move: Move = Move()

	@Before
	fun setup() {
		activity = ActivityMock(MovesActivity::class).create()

		val detail = Detail("desc", 3, 2.7)
		move.detailList.add(detail)

		box = DetailBox(
			activity,
			move,
			detail.description,
			detail.amount,
			detail.value,
			detail.conversion,
		)

		box.tag = "detail"
	}

	@Test
	fun addDetailBox() {
		val layout = LinearLayout(activity)
		layout.addView(box)

		assertNotNull(layout.findViewWithTag("detail"))

		val description = box.findViewById<TextView>(R.id.detail_description)
		assertNotNull(description)
		assertThat(description.text.toString(), `is`("desc"))

		val amount = box.findViewById<TextView>(R.id.detail_amount)
		assertNotNull(amount)
		assertThat(amount.text.toString(), `is`("3"))

		val value = box.findViewById<TextView>(R.id.detail_value)
		assertNotNull(value)
		assertThat(value.text.toString(), `is`("2.70".getDecimal()))

		val remove = box.findViewById<Button>(R.id.detail_remove)
		assertNotNull(remove)
	}

	@Test
	fun removeDetailBox() {
		val layout = LinearLayout(activity)
		layout.addView(box)

		box.findViewById<Button>(R.id.detail_remove)
			.performClick()

		assertThat(move.detailList.size, `is`(0))
		assertNull(layout.findViewWithTag("detail"))
	}
}
